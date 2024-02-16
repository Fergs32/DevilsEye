using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dox.Components.Tools.PortScan
{
    internal interface PortScannerInterface
    {
        /*
         * This will save the scan results to a file
         */
        public void SaveScanResults(Span<int> openports, Span<int> closedports, string? ip);
    }

    internal interface PortScannerHistory
    {
        /*
         * This will retreive the scan history
         */
        public void RetreiveScanHistory();
    }
}
