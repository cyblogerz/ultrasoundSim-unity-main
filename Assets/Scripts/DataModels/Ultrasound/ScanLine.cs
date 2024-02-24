using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace DataModels.Ultrasound
{
    using UnityEngine;
    using System.Collections;
    public class ScanLine : IEnumerable<ScanPoint>
    {
        private IList<ScanPoint> _points;
        public readonly Vector3 origin; //probe

        public ScanLine(Vector3 origin)
        {
            this.origin = origin;
            _points =  new List<ScanPoint>();
        }

        public void AddPoints(ICollection<ScanPoint> points)
        {
            foreach (var point in points)
            {
                AddPoint(point);
            }
            
        }
        public void AddPoint(ScanPoint point)
        {
            _points.Add(point);
        }

        public ReadOnlyCollection<ScanPoint> GetPoints()
        {
            return new ReadOnlyCollection<ScanPoint>(_points);
        }
        
        public IEnumerator<ScanPoint> GetEnumerator()
        {
            return _points.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override string ToString()
        {
            return _points.Aggregate("Scan Line", (current, point) => current + string.Format("\n{0}", point));
        }
    }
}