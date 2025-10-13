using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;

namespace UsersInfraestrutura
{
    public class UserDBContext : DbContext
    {
        private IConfiguration _configuration;

        public DbSet<UsersDomain.Entidades.Users> Users { get; set; }
        public DbSet<UsersDomain.Entidades.Agendamento> Agendamentos { get; set; }
        public DbSet<UsersDomain.Entidades.Servicos> Services { get; set; }
        public DbSet<UsersDomain.Entidades.Barbeiro> Barbeiros { get; set; }
        public UserDBContext(IConfiguration configuration, DbContextOptions options) : base(options)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof (configuration));
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var typeDatabase = _configuration["TypeDatabase"];
            var connectionString = _configuration.GetConnectionString(typeDatabase);

            if (typeDatabase == "SqlServer")
            {
                optionsBuilder.UseSqlServer(connectionString);
            }
        }
    }
}
