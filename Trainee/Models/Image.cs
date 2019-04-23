namespace Trainee.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Image")]
    public partial class Image
    {
        [Key]
        public int Idimg { get; set; }

        [StringLength(1000)]
        public string Imagename { get; set; }

        [StringLength(100)]
        public string Theme { get; set; }

        [StringLength(100)]
        public string Link { get; set; }

        [StringLength(50)]
        public string FromImg { get; set; }

        public bool? censor { get; set; }

        public virtual ThemeImage ThemeImage { get; set; }
    }
}
