namespace TodoList.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("LOAICONGVIEC")]
    public partial class LOAICONGVIEC
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [StringLength(50)]
        public string MALOAI { get; set; }

        public string TENLOAI { get; set; }

        public string MOTALOAI { get; set; }

        [Column(TypeName = "date")]
        public DateTime NGAYTAOLOAI { get; set; }

        [Column(TypeName = "date")]
        public DateTime NGAYCAPNHATLOAI { get; set; }

        [StringLength(50)]
        public string MATAIKHOAN { get; set; }
    }
}
