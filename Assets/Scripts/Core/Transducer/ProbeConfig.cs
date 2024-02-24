using UnityEngine;

namespace Core.Transducer
{
    public struct PosAndRot
    {
        public Vector3 Position;
        public Quaternion Rotation;
    }
    public class ProbeConfig
    {
        private PosAndRot _posAndRot;
        private float _maxDist;
        private float _minDist;
        private float _arcSizeDeg;
        private float _gain;
        private int _numScanLines;
        private int _pointsPerScanline;

        public ProbeConfig()
        {
            this.SetPosition(Vector3.zero);
            this.SetRotation(Quaternion.AngleAxis(0f,Vector3.forward));
            this._minDist = float.Epsilon;
            this._maxDist = 10f;
            this._arcSizeDeg = 75;
            this._numScanLines = 40;
            this._pointsPerScanline = 20;
            this._gain = 1f;
        }
        
        public ProbeConfig (ProbeConfig config) {
            this.SetPosition(config!.GetPosition());
            this.SetRotation(config.GetRotation());

            this._maxDist 			= config.GetMaxScanDistance();
            this._minDist			= config.GetMinScanDistance();
            this. _arcSizeDeg 		= config.GetArcSizeInDegrees();
            this. _pointsPerScanline 		= config.GetPointsPerScanline();
            this._numScanLines 		= config.GetNumberOfScanlines();
            this._gain		= config.GetGain();
        }

        
        public void SetRotation (Quaternion rotation) {
            this._posAndRot.Rotation = rotation;
        }
        public Quaternion GetRotation ()
        {
            return this._posAndRot.Rotation;
        }
        public void SetPosition (Vector3 position)
        {
            this._posAndRot.Position = position;
        }

      
        public Vector3 GetPosition ()
        {
            return this._posAndRot.Position;
        }

        public ProbeConfig(Transform probeTransform)
        {
	        if (!probeTransform)
	        {
		        this.SetRotation(probeTransform.rotation);
		        this.SetPosition(probeTransform.position);

		        this._minDist = float.Epsilon;
		        this._maxDist = 10f;
		        this._arcSizeDeg = 75;
		        this._numScanLines = 40;
		        this._pointsPerScanline = 40;
		        this._gain = 1.0f;
	        }
        }

        public void SetMinScanDistance(float min) {
		_minDist = Mathf.Clamp(min, float.Epsilon, float.MaxValue);
    }
    

    public float GetMinScanDistance() {
        return _minDist;
    }
    public void SetMaxScanDistance(float max) {
        _maxDist = Mathf.Clamp(max, float.Epsilon, float.MaxValue);

    }
    
   
    public float GetMaxScanDistance() {
        return _maxDist;
    }

	
	public void SetArcSizeInDegrees(float degrees) {
		_arcSizeDeg = Mathf.Clamp(degrees, 0f, 180f);
	}

	
	public float GetArcSizeInDegrees()
	{
		return _arcSizeDeg;
	}

	
	public void SetNumberOfScanlines(int count) {
		_numScanLines = Mathf.Clamp(count, 0, int.MaxValue);
	}

	
	public int GetNumberOfScanlines()
	{
		return _numScanLines;
	}


	public void SetPointsPerScanline(int count) {
		_pointsPerScanline = count;
	}


	public int GetPointsPerScanline()
	{
		return _pointsPerScanline;
	}


	public void SetGain(float gain) {

		this._gain = gain;
	}


	public float GetGain() {
		return this._gain;
	}
}
    }
    
    
    
  
