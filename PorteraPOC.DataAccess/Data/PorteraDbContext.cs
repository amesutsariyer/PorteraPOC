using Microsoft.EntityFrameworkCore;
using PorteraPOC.Entity;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PorteraPOC.DataAccess.Data
{
    public class PorteraDbContext : DbContext
    {
        public PorteraDbContext(DbContextOptions<PorteraDbContext> options) : base(options)
        {
     
        }
        public DbSet<Pilot> Ids { get; set; }
       

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Pilot>().ToTable("Ids");
            //MockData(modelBuilder);
        }

        //public void MockData(ModelBuilder modelBuilder)
        //{
     
        //    for (int i = 0; i < 400; i++)
        //    {
        //        modelBuilder.Entity<Pilot>().HasData(new Pilot()
        //        {
        //            Id = RandomString(),
        //            SerialNumber = Guid.NewGuid()
        //        });
        //    }
        //}
        //private static Random random = new Random();
        //public static string RandomString()
        //{
        //    const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        //    return new string(Enumerable.Repeat(chars, 11)
        //      .Select(s => s[random.Next(s.Length)]).ToArray());
        //}
    }
}
