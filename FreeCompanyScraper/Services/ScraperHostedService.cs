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
        private bool _running = false;
       
        public ScraperHostedService(Scraper.Services.ILodestoneScraper lodestoneScraper)
        {
            _lodestoneScraper = lodestoneScraper ?? throw new ArgumentException(nameof(lodestoneScraper));
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (!_running)
                {
                    _running = true;
                    await _lodestoneScraper.Run();
                    _running = false;
                }
                await Task.Delay(86400000, stoppingToken);
            }
        }
    }
}
