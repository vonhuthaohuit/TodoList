using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace TodoList.Models
{
    public partial class QL_CONGVIEC : DbContext
    {
        public QL_CONGVIEC()
            : base("name=QL_CONGVIEC")
        {
        }

        public virtual DbSet<CONGVIEC> CONGVIECs { get; set; }
        public virtual DbSet<LOAICONGVIEC> LOAICONGVIECs { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<TAIKHOAN> TAIKHOANs { get; set; }
        public virtual DbSet<THONGTINCANHAN> THONGTINCANHANs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CONGVIEC>()
                .Property(e => e.MACONGVIEC)
                .IsUnicode(false);

            modelBuilder.Entity<CONGVIEC>()
                .Property(e => e.MATAIKHOAN)
                .IsUnicode(false);

            modelBuilder.Entity<CONGVIEC>()
                .Property(e => e.MALOAI)
                .IsUnicode(false);

            modelBuilder.Entity<LOAICONGVIEC>()
                .Property(e => e.MALOAI)
                .IsUnicode(false);

            modelBuilder.Entity<LOAICONGVIEC>()
                .Property(e => e.MATAIKHOAN)
                .IsUnicode(false);

            modelBuilder.Entity<TAIKHOAN>()
                .Property(e => e.MATAIKHOAN)
                .IsUnicode(false);

            modelBuilder.Entity<TAIKHOAN>()
                .Property(e => e.TENTAIKHOAN)
                .IsUnicode(false);

            modelBuilder.Entity<TAIKHOAN>()
                .Property(e => e.MATKHAU)
                .IsUnicode(false);

            modelBuilder.Entity<THONGTINCANHAN>()
                .Property(e => e.MATHONGTIN)
                .IsUnicode(false);

            modelBuilder.Entity<THONGTINCANHAN>()
                .Property(e => e.CMND)
                .IsUnicode(false);

            modelBuilder.Entity<THONGTINCANHAN>()
                .Property(e => e.SDT)
                .IsUnicode(false);

            modelBuilder.Entity<THONGTINCANHAN>()
                .Property(e => e.MATAIKHOAN)
                .IsUnicode(false);
        }
    }
}
