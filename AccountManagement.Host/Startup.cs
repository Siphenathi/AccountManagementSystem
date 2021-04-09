using AccountManagementSystem.Service;
using AccountManagementSystem.Service.Interface;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
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

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddCors(options =>
			{
				options.AddPolicy("default",
					builder => builder.AllowAnyOrigin()
					.AllowAnyMethod()
					.AllowAnyHeader());
			});
			services.AddControllers(options => options.EnableEndpointRouting = false);

			string connectionString = Configuration.GetConnectionString("AMSConnection");
			services.AddScoped<IPersonRepository>(c => new PersonRepository(connectionString));
			services.AddScoped<IAccountRepository>(c => new AccountRepository(connectionString));
			services.AddScoped<ITransactionRepository>(c => new TransactionRepository(connectionString));
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			//app.UseStaticFiles();
			app.UseRouting();
			app.UseCors("default");

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}");
			});
		}
	}
}
