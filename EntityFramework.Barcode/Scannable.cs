using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using BarcodeLib;
using EntityFrameworkCore.Triggers;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;

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
                BarcodeConfig config = GetConfig(entry.Service);
                if (string.IsNullOrEmpty(entry.Entity.BarcodeEntry)) entry.Entity.BarcodeEntry = Generator.Generate();
                entry.Entity.Barcode.Alignment = config.Alignment;
                entry.Entity.BarcodeImage = ToBase64(entry.Entity.Barcode.Encode(
                    config.BarcodeType, 
                    entry.Entity.Barcode.RawData, 
                    Color.Black, 
                    Color.White, 
                    entry.Entity.Barcode.Width, 
                    entry.Entity.Barcode.Height)
                );
            };
            Triggers<Scannable>.Updating += entry => 
            {
                if (!string.IsNullOrEmpty(entry.Entity.BarcodeEntry))
                {
                    BarcodeConfig config = GetConfig(entry.Service);
                    entry.Entity.BarcodeImage = ToBase64(entry.Entity.Barcode.Encode(
                        config.BarcodeType, 
                        entry.Entity.Barcode.RawData, 
                        Color.Black, 
                        Color.White, 
                        entry.Entity.Barcode.Width, 
                        entry.Entity.Barcode.Height)
                    );
                }
            };
        }

        private static BarcodeConfig GetConfig(IServiceProvider provider)
        {
            if (provider == null) return new BarcodeConfig();
            var options = provider.GetService<IOptions<BarcodeConfig>>();
            BarcodeConfig config = options?.Value ?? new BarcodeConfig();
            return config;
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
