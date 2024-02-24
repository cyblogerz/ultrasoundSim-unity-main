using System.Collections.Generic;
using System.Collections;
using System.Collections.ObjectModel;
using Core.Transducer;

namespace DataModels.Ultrasound
{
    
    public class ScanData : IEnumerable<ScanLine>
    {
        private IList<ScanLine> _scanlines;
        private readonly ProbeConfig _probeConfig;

        public ScanData(ProbeConfig config)
        {
            this._probeConfig = config;
            _scanlines = new List<ScanLine>();
        }

        public void AddScanlines (ICollection<ScanLine> scanlines) {

            foreach (ScanLine s in scanlines) {
                AddScanLine(s);
            }
        }

        public void AddScanLine(ScanLine s)
        {
            _scanlines.Add(s);
        }
        
        public ReadOnlyCollection<ScanLine> GetScanlines() {
            return new ReadOnlyCollection<ScanLine>(_scanlines);
        }
        
        public ProbeConfig GetProbeConfig() {
            return new ProbeConfig(_probeConfig);
        }
        public IEnumerator<ScanLine> GetEnumerator()
        {
            return _scanlines.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}