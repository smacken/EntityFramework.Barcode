using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.IO;
using BarcodeLib;
using EntityFrameworkCore.Triggers;

namespace EntityFramework.Barcode
{
    public abstract class Scannable
    {
        public virtual BarcodeLib.Barcode Barcode { get; set; }
        public virtual string BarcodeImage { get; set; }
        
        /// <summary>
        /// The barcode string if not to be generated automatically.
        /// On Update will replace barcode if set.
        /// </summary>
        [NotMapped]
        public virtual string BarcodeEntry { get; set; }

        [NotMapped]
        public virtual Image Image => FromBase64(BarcodeImage);

        static Scannable() {
            Triggers<Scannable>.Inserting += entry => 
            {
                var code = string.IsNullOrEmpty(entry.Entity.BarcodeEntry) 
                    ? new BarcodeLib.Barcode() 
                    : new BarcodeLib.Barcode(entry.Entity.BarcodeEntry);
                entry.Entity.Barcode = code;
                entry.Entity.BarcodeImage = ToBase64(entry.Entity.Barcode.Encode(
                    BarcodeLib.TYPE.UPCA, 
                    entry.Entity.Barcode.RawData, 
                    Color.Black, 
                    Color.White, 
                    290, 120)
                );
            };
            Triggers<Scannable>.Updating += entry => 
            {
                if (!string.IsNullOrEmpty(entry.Entity.BarcodeEntry))
                {
                    entry.Entity.Barcode = new BarcodeLib.Barcode(entry.Entity.BarcodeEntry);
                    entry.Entity.BarcodeImage = ToBase64(entry.Entity.Barcode.Encode(
                        BarcodeLib.TYPE.UPCA, 
                        entry.Entity.Barcode.RawData, 
                        Color.Black, 
                        Color.White, 
                        290, 120)
                    );
                }
            };
        }

        public static string ToBase64(Image image)
        {
            using (var memory = new MemoryStream())
            {
                image.Save(memory, image.RawFormat);
                return Convert.ToBase64String(memory.ToArray());
            }
        }

        public static Image FromBase64(string base64String)
        {
            byte[] imageBytes = Convert.FromBase64String(base64String);
            using (var memory = new MemoryStream(imageBytes, 0, imageBytes.Length))
            {
                return Image.FromStream(memory, true);
            }
        }
    }
}
