using appstore.notification.api.Interfaces;
using appstore.notification.api.Services;
using Apple.Receipt.Parser.Modules;

namespace appstore.notification.api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddSingleton<INotificationProcessor, NotificationProcessor>();
            builder.Services.AddSingleton<ISubsciptionService, SubsciptionService>();
            builder.Services.AddControllers();
            builder.Services.AddSwaggerGen();
            builder.Services.RegisterAppleReceiptParser();

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseSwagger();
            app.UseSwaggerUI();


            app.MapControllers();

            app.Run();
        }
    }
}