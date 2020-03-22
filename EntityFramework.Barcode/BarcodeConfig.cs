using System;
using BarcodeLib;

namespace EntityFramework.Barcode
{
    public class BarcodeConfig
    {
        public BarcodeLib.TYPE BarcodeType { get; set; }
        public bool IncludeLabel { get; set; }
    }
}