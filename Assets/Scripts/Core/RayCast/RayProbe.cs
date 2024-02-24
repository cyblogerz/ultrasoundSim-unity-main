using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Core.Display.Interfaces;
using Core.Transducer;
using DataModels.Ultrasound;

namespace Core.RayCast
{
    public class RayProbe
    {
        protected readonly RayOrganCuller culler;
        protected readonly GameObject probeObject;
        protected readonly IProbeOutput output;

        public RayProbe(GameObject probe, IProbeOutput output)
        {
            this.probeObject = probe;
            this.output = output;
            this.culler = new RayOrganCuller();
        }

        public void PopulateData(ref ScanData data)
        {
            EstablishScanningPlane(ref data);
            IList<GameObject> culledOrganList =
                culler.HitableOrgansOnScanlines(data.GetScanlines(), data.GetProbeConfig());
            ScanPointsForOrgans(ref data, culledOrganList);
        }

        private void ScanPointsForOrgans(ref ScanData data, IList<GameObject> organList)
        {
            int scanlineIndex = 0;
            int totalScanlines = data.GetProbeConfig().GetNumberOfScanlines();
            foreach (var scanline  in data)
            {
                float pulseIntensity = data.GetProbeConfig().GetGain();

                foreach (var point in scanline )
                {
                    foreach (var gameObject in organList)
                    {
                        var target = point.GetWorldSpaceLoc();
                        var collider = gameObject.GetComponent<Collider>();
                        bool hit = CollisionUtils.IsContained(target, collider);

                        if (hit)
                        {
                            MaterialProps organProps = gameObject.GetComponent<MaterialProps>();
                            float pointIntensity = IntensityAtPoint(pulseIntensity, organProps);
                            point.SetBrightness(pointIntensity);
                            pulseIntensity =
                                PulseIntensityAfterPoint(pulseIntensity, organProps, data.GetProbeConfig());
                        }
                    }
                }
            }
        }

        private void EstablishScanningPlane(ref ScanData data)
        {
            ProbeConfig config = data.GetProbeConfig();
            float nearZ = config.GetMinScanDistance();
            float farZ = config.GetMaxScanDistance();
            float arcSizeDegrees = config.GetArcSizeInDegrees();
            int scanlines = config.GetNumberOfScanlines();
            int pointsPerScanline = config.GetPointsPerScanline();

            for (int i = 0; i < scanlines; ++i)
            {
                ScanLine scanline = new ScanLine(config.GetPosition());
                float angleInDegrees = -(arcSizeDegrees / 2) + i * arcSizeDegrees / (scanlines - 1);
                float angleInRadians = angleInDegrees * Mathf.Deg2Rad;
                Vector2 trajectory = new Vector2(Mathf.Sin(angleInRadians),Mathf.Cos(angleInRadians));

                for (int j = 0; j < pointsPerScanline; ++j)
                {
                    float d = nearZ + j * (farZ - nearZ) / (pointsPerScanline - 1);
                    Vector2 positionOnPlane = d * trajectory;
                    Vector3 positionInWorldSpace = WorldSpaceFromProjected(positionOnPlane, config);
                    ScanPoint point = new ScanPoint(positionInWorldSpace, positionOnPlane);
                    scanline.AddPoint(point);

                }
                
                data.AddScanLine(scanline);
            }
            
            
        }

        private float IntensityAtPoint(float pulseInensity, MaterialProps orgProps)
        {
            float intensity = (pulseInensity * orgProps.echogenecity) - 0.02f * orgProps.attenuation;
            return Mathf.Clamp(intensity, 0f, 1f);

        }

        private float PulseIntensityAfterPoint(float prevIntense, MaterialProps prevOrgan,ProbeConfig config)
        {
            // during calculating energy lose , gotta normalize
            float resolutionCoefficient = config.GetPointsPerScanline();
            float energyLost = (prevOrgan.echogenecity + prevOrgan.attenuation) / resolutionCoefficient;
            float newIntesity = prevIntense - energyLost;
            return newIntesity;
        }

        private Vector3 WorldSpaceFromProjected(Vector2 positionInPlane,ProbeConfig config)
        {
            Vector3 worldPosition = new Vector3(positionInPlane.x,0,positionInPlane.y);
            worldPosition = config.GetPosition();
            worldPosition += config.GetPosition();
            return worldPosition;
        }

    }
}