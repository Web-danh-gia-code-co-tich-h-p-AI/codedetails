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

        [Required(ErrorMessage = "Tên không được để trống.")]
        [StringLength(100, ErrorMessage = "Tên không được vượt quá 100 ký tự.")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Tên người dùng không được để trống.")]
        [StringLength(50, ErrorMessage = "Tên người dùng không được vượt quá 50 ký tự.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Email không được để trống.")]
        [EmailAddress(ErrorMessage = "Địa chỉ email không hợp lệ.")]
        public string Email { get; set; }

        [Column(TypeName = "LONGTEXT")]
        public string? CodeDetails { get; set; }
    }
}
