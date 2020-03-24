using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using BarcodeLib;
using EntityFrameworkCore.Triggers;

namespace EntityFramework.Barcode
{
    public abstract class Scannable
    {
        public virtual string BarcodeImage { get; set; }
        
        /// <summary>
        /// The barcode string if not to be generated automatically.
        /// On Update will replace barcode if set.
        /// </summary>
        public virtual string BarcodeEntry { get; set; }

        [NotMapped]
        public virtual BarcodeLib.Barcode Barcode => new BarcodeLib.Barcode(BarcodeEntry);

        [NotMapped]
        public virtual Image Image => FromBase64(BarcodeImage);

        static Scannable() {
            Triggers<Scannable>.Inserting += entry => 
            {
                if (string.IsNullOrEmpty(entry.Entity.BarcodeEntry)) entry.Entity.BarcodeEntry = Generator.Generate();
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
                image.Save(memory, ImageFormat.Jpeg);
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
