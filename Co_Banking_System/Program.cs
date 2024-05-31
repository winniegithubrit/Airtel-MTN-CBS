using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Security.Cryptography.X509Certificates;
using Co_Banking_System.Services;
using Co_Banking_System.Options;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var configuration = builder.Configuration;

// Configure logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

// Configure Airtel API settings
builder.Services.Configure<AirtelApiOptions>(configuration.GetSection("AirtelApi"));
builder.Services.AddSingleton<AirtelApiClient>();

// Configure MTN MoMo API settings
builder.Services.Configure<MoMoApiOptions>(configuration.GetSection("MtnMomoSettings"));
builder.Services.AddHttpClient<MtnMomoService>();
builder.Services.AddHttpClient<MtnDisbursementService>(client =>
{
    client.BaseAddress = new Uri("https://sandbox.momodeveloper.mtn.com");
    client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", configuration["DisbursementSettings:ApiKey"]);
});

builder.Services.AddControllers();

// Configure Kestrel to use HTTPS and the PFX certificate
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ListenLocalhost(5000, listenOptions =>
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
