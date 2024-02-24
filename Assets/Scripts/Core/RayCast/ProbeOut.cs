// Implements raycasting for ultrasound simulation
using UnityEngine;
using System.Collections;
using Core.Display.Interfaces;
using Core.RayCast;
using Core.Transducer;
using DataModels.Ultrasound;


public class ProbeOut : IProbeOutput
{
    protected readonly GameObject probeObject;
    protected readonly RayProbe probe;
    
    public ProbeOut(GameObject gameObj)
    {
        this.probeObject = gameObj;
        probe = new RayProbe(probeObject,this);
    }
    
    public ScanData sendScanData()
    {
        ProbeConfig currConf = probeObject.GetComponent<RayBehaviour>().GetProbeConfig();
        ScanData data = new ScanData(currConf);
        probe.PopulateData(ref data);
        return data;
    }
}