using System;
using BarcodeLib;

namespace EntityFramework.Barcode
{
    public class BarcodeConfig
    {
        public BarcodeLib.TYPE BarcodeType { get; set; } = BarcodeLib.TYPE.UPCA;
        public bool IncludeLabel { get; set; } = true;
        public BarcodeLib.AlignmentPositions Alignment { get; set; } = AlignmentPositions.CENTER;
    }
}