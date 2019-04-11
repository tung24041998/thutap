namespace Trainee.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("User")]
    public partial class User
    {
        public int Id { get; set; }

        [Required]
        [StringLength(1000)]
        public string Username { get; set; }
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Email không chính xác ")]
        [StringLength(100)]
        public string Email { get; set; }

        [StringLength(1000)]
        public string Address { get; set; }

        [Required]
        [StringLength(50)]
        public string Passwork { get; set; }

        [StringLength(50)]
        public string right { get; set; }
    }
}
