namespace TodoList.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CONGVIEC")]
    public partial class CONGVIEC
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [StringLength(50)]
        public string MACONGVIEC { get; set; }

        public string TENCONGVIEC { get; set; }

        [Column(TypeName = "text")]
        public string MOTACONGVIEC { get; set; }

        public bool TRANGTHAIHOANTHANH { get; set; }

        [Column(TypeName = "date")]
        public DateTime NGAYTAOCONGVIEC { get; set; }

        [Column(TypeName = "date")]
        public DateTime NGAYCAPNHATCONGVIEC { get; set; }

        [StringLength(50)]
        public string MATAIKHOAN { get; set; }

        [StringLength(50)]
        public string MALOAI { get; set; }
    }
}
