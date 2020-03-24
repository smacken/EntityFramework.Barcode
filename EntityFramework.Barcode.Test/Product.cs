using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntityFramework.Barcode.Test
{
    [Table("Product")]
    public class Product : Scannable
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}