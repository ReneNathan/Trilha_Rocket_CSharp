using Microsoft.EntityFrameworkCore;
using PassIn.Infrastructure.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PassIn.Infrastructure
{
    public class PassInDbContext : DbContext
    {
        public DbSet<Event> Events { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=C:\\ProjetosDev\\C#\\Trilha C# Rocket\\BD\\PassInDb.db");
        }
    }
}
