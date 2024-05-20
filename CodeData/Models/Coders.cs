using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeData.Models
{
    public class Coders
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IDCode { get; set; } // Khóa chính tự động tăng

        public string? ID { get; set; } 

        public string? Name { get; set; }

        public string UserName { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Column(TypeName = "LONGTEXT")]
        public string? CodeDetails { get; set; }
    }
}
