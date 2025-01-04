using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using UserLoginRegisterService.EntityLayer.Concrete;

namespace UserLoginRegisterService.DataAccessLayer.Concrete
{
	public class Context : DbContext
	{
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlServer("data source=MILANOPC;initial catalog=DisHastalikDB;user id=ps;password=12345Aa!;MultipleActiveResultSets=True;App=EntityFramework;TrustServerCertificate=True");
		}
		public DbSet<User> Users { get; set; }
		public DbSet <Role> Roles { get; set; }
	}
}
