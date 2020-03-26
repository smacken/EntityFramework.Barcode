using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace EntityFramework.Barcode.Test
{
    public class DbFixture
    {
        public DbFixture()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddDbContext<Context>(options => 
                options.UseInMemoryDatabase(databaseName: "Database"), ServiceLifetime.Transient);
            IOptions<BarcodeConfig> options = Options.Create<BarcodeConfig>(new BarcodeConfig());
            serviceCollection.AddSingleton<IOptions<BarcodeConfig>>(options);
            ServiceProvider = serviceCollection.BuildServiceProvider();
        }

        public ServiceProvider ServiceProvider { get; private set; }
    }
}
