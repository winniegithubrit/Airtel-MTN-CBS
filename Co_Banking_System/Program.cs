using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Security.Cryptography.X509Certificates;
using Co_Banking_System.Services;
using Co_Banking_System.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var configuration = builder.Configuration;
builder.Services.Configure<AirtelApiOptions>(configuration.GetSection("AirtelApi"));
builder.Services.AddSingleton<AirtelApiClient>();
builder.Services.AddHttpClient();
builder.Services.AddControllers();

// Configure Kestrel to use HTTPS and the PFX certificate
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ListenLocalhost(5058, listenOptions =>
    {
        listenOptions.UseHttps(httpsOptions =>
        {
            var certificatePath = configuration["AirtelApi:CertificatePath"];
            var certificatePassword = configuration["AirtelApi:CertificatePassword"];

            if (!string.IsNullOrEmpty(certificatePath) && !string.IsNullOrEmpty(certificatePassword))
            {
                httpsOptions.ServerCertificate = new X509Certificate2(certificatePath, certificatePassword);
                httpsOptions.SslProtocols = System.Security.Authentication.SslProtocols.Tls12 | System.Security.Authentication.SslProtocols.Tls13;
            }
            else
            {
                Console.WriteLine("Certificate path or password is null or empty.");
            }
        });
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run();
