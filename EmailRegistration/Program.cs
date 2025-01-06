using EmailRegistration.DataAccess;
using EmailRegistration.Models;
using EmailRegistration.Services;
using EmailRegistration.Services.Imp;
using EmailRegistration.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EmailRegistration
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            builder.Services.Configure<RabbitMqSettings>(builder.Configuration.GetSection("RabbitMqSettings"));
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DbContext")));

            builder.Services.AddIdentity<UserModel, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();
            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddScoped<IVerificationService, VerificationService>();
            builder.Services.AddScoped<IEmailAuthQueueProducer, EmailAuthQueueProducer>();
            builder.Services.AddScoped<IAuthorizationUserService, AuthorizationUserService>();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddLogging(logging =>
            {
                logging.AddConsole();
                logging.SetMinimumLevel(LogLevel.Debug);
            });

            var app = builder.Build();
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseHttpsRedirection();
            //app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.UseCors(x =>
            {
                x.WithHeaders().AllowAnyHeader();
                x.WithMethods().AllowAnyMethod();
                x.WithOrigins("http://localhost:3000");
                x.AllowCredentials();
            });

            app.Run();
        }
    }
}
