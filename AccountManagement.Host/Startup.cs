using AccountManagementSystem.Service;
using AccountManagementSystem.Service.Interface;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AccountManagementSytem.Host
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }
		//private static string connectionString = Configuration.GetConnectionString("");

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddControllers();
			services.AddSingleton<IPersonRepository, PersonRepository>();
			services.AddScoped<IPersonRepository>(c => new PersonRepository(Configuration.GetConnectionString("AMSConnection")));

			services.AddSingleton<IAccountRepository, AccountRepository>();
			services.AddScoped<IAccountRepository>(c => new AccountRepository(Configuration.GetConnectionString("AMSConnection")));

			services.AddSingleton<ITransactionRepository, TransactionRepository>();
			services.AddScoped<ITransactionRepository>(c => new TransactionRepository(Configuration.GetConnectionString("AMSConnection")));
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseHttpsRedirection();

			app.UseRouting();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
