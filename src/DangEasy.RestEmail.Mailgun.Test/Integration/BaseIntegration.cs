using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace DangEasy.RestEmail.Mailgun.Test.Integration
{
    public class BaseIntegration : IDisposable
    {
        protected IConfigurationRoot Configuration;

        public BaseIntegration()
        {
            var sharedFolder = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "..", "Example.Console");

            var builder = new ConfigurationBuilder()
                          .SetBasePath(sharedFolder)
                          .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            Configuration = builder.Build();

        }


        public void Dispose()
        {
        }
    }
}