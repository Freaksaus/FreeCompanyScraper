using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Web.Services
{
    public class ScraperHostedService : BackgroundService
    {
        private readonly Scraper.Services.ILodestoneScraper _lodestoneScraper;
       
        public ScraperHostedService(Scraper.Services.ILodestoneScraper lodestoneScraper)
        {
            _lodestoneScraper = lodestoneScraper ?? throw new ArgumentException(nameof(lodestoneScraper));
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await _lodestoneScraper.Run();
                await Task.Delay(86400000, stoppingToken);
            }
        }
    }
}
