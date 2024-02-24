using System.Collections.Generic;
using Core.Transducer;
using DataModels.Ultrasound;
using UnityEngine;

namespace Core.RayCast
{
    // this class is responsible for detecting which organs can possibly be hit by raycasts
    public class RayOrganCuller
    {
        private readonly IList<GameObject> allOrgsInScene;
        private SortedDictionary<float, GameObject> visibleOrgs;
        private IDictionary<GameObject, float> reverseLookup;

        private int linesPerFrame = 5;
        private float expirationTime = 5f; // how long to wait to remove an org

        private int scanLineIndex;


        public RayOrganCuller()
        {
            visibleOrgs = new SortedDictionary<float, GameObject>();
            reverseLookup = new Dictionary<GameObject, float>();
            allOrgsInScene = new List<GameObject>();
            object[] allGameObjsInScene = GameObject.FindObjectsOfType<GameObject>();

            foreach (var obj in allGameObjsInScene)
            {
                GameObject gameObject = (GameObject)obj;
                if (null != gameObject.GetComponent<MaterialProps>())
                {
                    allOrgsInScene.Add(gameObject);
                }
            }
            
        }

        public IList<GameObject> HitableOrgansOnLine(IList<ScanLine> scanlines, ProbeConfig config)
        {
	        CheckScanlines(scanlines);
	        RemoveExpired();
	        return new List<GameObject>(visibleOrgs.Values);

        }

        private void CheckScanlines(IList<ScanLine> scanlines) {
		int scanlineCount = scanlines.Count;
		int scanlineStepSize = (scanlineCount / (linesPerFrame + 1));
		while (scanlineCount % scanlineStepSize == 0) {
			scanlineStepSize++;
		}

		for (int i = 0; i < linesPerFrame; ++i) {
			scanLineIndex = (scanLineIndex + scanlineStepSize) % scanlineCount;
			IList<GameObject> organsToAdd = (ValidOrgsOnLine(scanlines[scanLineIndex]));
			foreach (GameObject organ in organsToAdd) {
				if (!reverseLookup.ContainsKey(organ)) {
					float keyTime = Time.time;
					while (visibleOrgs.ContainsKey(keyTime)) {
						keyTime += 0.01f;
					}
					visibleOrgs.Add(keyTime, organ);
					reverseLookup.Add(organ, keyTime);
				}
				else {
					float oldTime = float.NegativeInfinity;
					reverseLookup.TryGetValue(organ, out oldTime);
					visibleOrgs.Remove(oldTime);
					reverseLookup.Remove(organ);
					float newTime = Time.time;
					while (visibleOrgs.ContainsKey(newTime)) {
						newTime += 0.01f;
					}
					visibleOrgs.Add(newTime, organ);
					reverseLookup.Add(organ, newTime);
				}
			}
		}
	}
        
        

        private IList<GameObject> ValidOrgsOnLine(ScanLine scanLine)
        {
            IList<ScanPoint> points = scanLine.GetPoints();
            Vector3 terminalPoint = points[points.Count - 1].GetWorldSpaceLoc();
            Ray ray = new Ray(scanLine.origin, terminalPoint - scanLine.origin);
            float raycastDistance = (terminalPoint - scanLine.origin).magnitude;
            RaycastHit[] hits = Physics.RaycastAll(ray, raycastDistance);
            IList<GameObject> validOrgans = new List<GameObject>();
            foreach(RaycastHit hit in hits) {
                if (allOrgsInScene.Contains(hit.collider.gameObject)) {
                    validOrgans.Add (hit.collider.gameObject);
                }
            }

            return validOrgans;
        }

        private void RemoveExpired()
        {
            IList<float> keysToRemove = new List<float>();
            IList<GameObject> organsToRemove = new List<GameObject>();

            foreach (var timeLastSeen in visibleOrgs.Keys)
            {
                float timeeElapsed = Time.time - timeLastSeen;
                if (timeeElapsed >= expirationTime)
                {
                    GameObject organToRemove = null;
                    visibleOrgs.TryGetValue(timeLastSeen, out organToRemove);
                }
                else
                {
                    break;
                }

                for (int i = 0; i < organsToRemove.Count; i++)
                {
                    visibleOrgs.Remove(keysToRemove[i]);
                    reverseLookup.Remove(organsToRemove[i]);

                }
            }
        }



    }
}