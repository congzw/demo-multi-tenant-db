using System.Linq;

namespace DemoApp.Services
{
    public class SeedService
    {
        private readonly DemoDbContext _dbContext;
        private readonly TenantContext _tenantContext;

        public SeedService(DemoDbContext dbContext, TenantContext tenantContext)
        {
            _dbContext = dbContext;
            _tenantContext = tenantContext;
        }

        public void Seed()
        {
            _dbContext.Database.EnsureCreated();
            var users = _dbContext.Users.ToList();
            if (users.Count > 0)
            {
                return;
            }
            _dbContext.Users.Add(new User() { Id = 1, Username = "admin" , FromSource = _tenantContext.Tenant});
            _dbContext.SaveChanges();
        }
    }
}
