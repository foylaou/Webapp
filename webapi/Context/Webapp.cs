using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using webapi.Entities;

namespace webapi.Context;

public partial class Webapp : DbContext
{
    public Webapp()
    {
    }

    public Webapp(DbContextOptions<Webapp> options)
        : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=foynas.synology.me;Port=5433;Database=Webapp;Username=postgres;Password=t0955787053S");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    //建立table 實例
    public DbSet<UserInfo> UserInfos { get; set; }
    

}
