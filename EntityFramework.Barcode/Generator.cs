using System;
using System.Text;

namespace EntityFramework.Barcode 
{
    public class Generator
    {
        public static string Generate(int length = 10, bool random=true)
        {
            if (random){
                var ran = new Random();
                var builder = new StringBuilder(length);
                for (int i = 0; i < length; i++)
                    builder.Append(ran.Next(9));
                return builder.ToString();
            }
            return string.Empty;
        }
    }
}