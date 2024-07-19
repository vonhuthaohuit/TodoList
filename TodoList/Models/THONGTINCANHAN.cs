namespace TodoList.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("THONGTINCANHAN")]
    public partial class THONGTINCANHAN
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [StringLength(50)]
        public string MATHONGTIN { get; set; }

        public string HOTEN { get; set; }

        [Column(TypeName = "date")]
        public DateTime? NGAYSINH { get; set; }

        [StringLength(20)]
        public string CMND { get; set; }

        [StringLength(11)]
        public string SDT { get; set; }

        [Column(TypeName = "date")]
        public DateTime? NGAYLAP { get; set; }

        [StringLength(50)]
        public string MATAIKHOAN { get; set; }
    }
}
