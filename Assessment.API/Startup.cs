using Assessment.Data.Interfaces;
using Assessment.Data.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Assessment.Data.Model;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Threading;
using MySql.Data.MySqlClient;

namespace Assessment.API
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
            WaitForDBInit(Configuration["ConnectionStrings:AssessmentDbContext"]);
            services.AddControllers();

            // Add DbContext
            services.AddCors();
            services.AddDbContext<AssessmentDbContext>(builder =>
            {
                builder.UseMySql(Configuration["ConnectionStrings:AssessmentDbContext"]);
            });

            #region Add repositories
            services.Add(new ServiceDescriptor(typeof(IProductRepository),
                                              typeof(ProductRepository),
                                              ServiceLifetime.Scoped));

            services.Add(new ServiceDescriptor(typeof(INewsRepository),
                                              typeof(NewsRepository),
                                              ServiceLifetime.Scoped));

            services.Add(new ServiceDescriptor(typeof(IShoppingCardRepository),
                                              typeof(ShoppingCardRepository),
                                              ServiceLifetime.Scoped));

            services.Add(new ServiceDescriptor(typeof(IUserShoppingCardStorageRepository),
                                              typeof(UserShoppingCardStorageRepository),
                                              ServiceLifetime.Scoped));
            #endregion

            #region Identity & Authentication

            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<AssessmentDbContext>()
                .AddDefaultTokenProviders();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = Configuration["JWT:ValidAudience"],
                    ValidIssuer = Configuration["JWT:ValidIssuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Secret"]))
                };
            });
            #endregion

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, AssessmentDbContext db)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            db.Database.Migrate();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private static void WaitForDBInit(string connectionString)
        {
            var connection = new MySqlConnection(connectionString);
            int retries = 1;
            while (retries < 7)
            {
                try
                {
                    Console.WriteLine("Connecting to db. Trial: {0}", retries);
                    connection.Open();
                    connection.Close();
                    break;
                }
                catch (MySqlException)
                {
                    Thread.Sleep((int)Math.Pow(2, retries) * 1000);
                    retries++;
                }
            }
        }
    }
}
