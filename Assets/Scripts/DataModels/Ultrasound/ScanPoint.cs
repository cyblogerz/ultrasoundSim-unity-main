using System;
using UnityEngine;

namespace DataModels.Ultrasound
{
    public class ScanPoint : IComparable
    {
        private readonly Vector3 _worldSpace;
        private readonly Vector2 _projectedPoint;
        private float _brightness;
        
        public ScanPoint(Vector3 worldSpace, Vector2 projectedPoint)
        {
            this._worldSpace = worldSpace;
            this._projectedPoint = projectedPoint;
            _brightness = 0f;
        }
        // here we define some getters.
        public Vector3 GetWorldSpaceLoc(){
            return this._worldSpace;
        }
        public Vector2 GetProjectedPoint(){
            return this._projectedPoint;
        }
        public float GetBrightness(){
            return this._brightness;
        }
        public void SetBrightness(float brightness){
            this._brightness = Mathf.Clamp(brightness, 0f, 1f);
        }

        public override bool Equals(object obj)
        {
            if (this == obj) return true;
            if (!(obj is ScanPoint)) return false;
            ScanPoint other = (ScanPoint)obj;
            return (other.GetProjectedPoint().Equals(this.GetProjectedPoint()) &&
                    other.GetWorldSpaceLoc().Equals(other.GetWorldSpaceLoc())
                );
        }

        public override int GetHashCode()
        {
            int rand = 19;
            int hash = rand  + this._projectedPoint.GetHashCode();
            hash = rand * hash + this._worldSpace.GetHashCode();
            return hash;
        }

        public int CompareTo(object obj) {
            if (this.Equals(obj)) return 0;
            if (!(obj is ScanPoint))
            {
                throw new ArgumentException("Can only compare UltrasoundPoints to other UltrasoundPoints!");
            }
            ScanPoint other = (ScanPoint)obj;
            float diff = this.GetProjectedPoint().magnitude - other.GetProjectedPoint().magnitude;
            return (int)(1000f * diff);
        }

    }
}