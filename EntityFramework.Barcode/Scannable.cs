﻿using System;
using BarcodeLib;

namespace EntityFramework.Barcode
{
    public abstract class Scannable
    {
        public virtual Barcode Barcode { get; set; }
        public virtual string BarcodeImage { get; set; }

        static Scannable() {
            Triggers<Scannable>.Inserting += entry => 
            {
                entry.Entity.Barcode = new BarcodeLib.Barcode();
                entry.Entity.BarcodeImage = ToBase64(entry.Entity.Barcode.Encode(BarcodeLib.TYPE.UPCA, "038000356216", Color.Black, Color.White, 290, 120));
            };
        }

        public string ToBase64(Image image)
        {
            using (var m = new MemoryStream())
            {
                image.Save(m, image.RawFormat);
                byte[] imageBytes = m.ToArray();
                string base64String = Convert.ToBase64String(imageBytes);
                return base64String;
            }
        }
    }
}