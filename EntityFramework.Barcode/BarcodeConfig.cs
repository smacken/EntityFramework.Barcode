using System;
using BarcodeLib;

namespace EntityFramework.Barcode
{
    public class BarcodeConfig
    {
        public BarcodeLib.Type BarcodeType { get; set; }
        public bool IncludeLabel { get; set; }
    }
}