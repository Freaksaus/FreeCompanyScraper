using LodestoneAPI.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace LodestoneAPI.Services
{
    public class LodestoneAPI : ILodestoneAPI
    {
        private readonly HttpClient _client;
        private readonly ILodestoneParser _lodestoneParser;
        private readonly APIOptions _options;

        private const int MAX_PAGES = 20;
        private const int MAX_RESULTS_PER_PAGE = 50;

        public LodestoneAPI(ILodestoneParser lodestoneParser, IOptions<APIOptions> options)
        {
            _lodestoneParser = lodestoneParser ?? throw new ArgumentException(nameof(lodestoneParser));
            _options = options.Value ?? throw new ArgumentException(nameof(options));

            _client = new HttpClient();
            _client.DefaultRequestHeaders.Add("User-Agent", "Free Company Scraper");
        }

        public async Task<Character> GetCharacter(string id)
        {
            var result = await GetCharacterPage(id);
            return result;
        }

        public async Task<IEnumerable<FreeCompanyEntry>> GetFreeCompanies(string serverName)
        {
            var result = new List<FreeCompanyEntry>();

            int page = 1;
            var companies = await GetFreeCompaniesPerPage(serverName, page);
            result.AddRange(companies);


            while (companies.Count == MAX_RESULTS_PER_PAGE && page < MAX_PAGES)
            {
                page++;
                companies = await GetFreeCompaniesPerPage(serverName, page);
                result.AddRange(companies);
            }

            return result;
        }

        public async Task<IEnumerable<FreeCompanyMemberEntry>> GetFreeCompanyMembers(string id)
        {
            var result = new List<FreeCompanyMemberEntry>();

            int page = 1;
            var members = await GetFreeCompanyMembersPerPage(id, page);
            if (members == null)
            {
                return result;
            }

            result.AddRange(members);

            while (members.Count == MAX_RESULTS_PER_PAGE && page < MAX_PAGES)
            {
                page++;
                members = await GetFreeCompanyMembersPerPage(id, page);
                result.AddRange(members);
            }

            return result;
        }

        private async Task<List<FreeCompanyEntry>> GetFreeCompaniesPerPage(string serverName, int page)
        {
            var filename = $"Companies/{serverName}-{page}.html";
            var url = $"https://na.finalfantasyxiv.com/lodestone/freecompany/?q=&worldname={serverName}&character_count=&activetime=&join=&house=&order=&page={page}";

            var html = await GetHtml(url, filename);
            if(string.IsNullOrWhiteSpace(html))
            {
                return null;
            }

            var result = await _lodestoneParser.ParseFreeCompanySearchPage(html);

            if (result.Count == 0)
            {
                //TODO: Refactor
                System.IO.File.Delete(System.IO.Path.Combine(_options.CacheDirectory, filename));
            }

            return result;
        }

        private async Task<List<FreeCompanyMemberEntry>> GetFreeCompanyMembersPerPage(string id, int page)
        {
            var filename = $"Members/{id}-{page}.html";
            var url = $"https://na.finalfantasyxiv.com/lodestone/freecompany/{id}/member/?page={page}";

            var html = await GetHtml(url, filename);
            if (string.IsNullOrWhiteSpace(html))
            {
                return null;
            }

            var result = await _lodestoneParser.ParseFreeCompanyMemberPage(html, id);

            if (result.Count == 0)
            {
                //TODO: Refactor
                System.IO.File.Delete(System.IO.Path.Combine(_options.CacheDirectory, filename));
            }

            return result;
        }

        private async Task<Character> GetCharacterPage(string id)
        {
            var filename = $"Characters/{id}.html";
            var url = $"https://na.finalfantasyxiv.com/lodestone/character/{id}/";

            var html = await GetHtml(url, filename);
            if (string.IsNullOrWhiteSpace(html))
            {
                return null;
            }

            var result = await _lodestoneParser.ParseCharacterPage(html, id);

            if (result == null)
            {
                //TODO: Refactor
                System.IO.File.Delete(System.IO.Path.Combine(_options.CacheDirectory, filename));
            }

            return result;
        }

        private async Task<string> GetHtml(string url, string filename)
        {
            var path = System.IO.Path.Combine(_options.CacheDirectory, filename);
            var directory = System.IO.Path.GetDirectoryName(path);
            if(!System.IO.Directory.Exists(directory))
            {
                System.IO.Directory.CreateDirectory(directory);
            }

            if(!System.IO.File.Exists(path))
            {
                System.Threading.Thread.Sleep(1000);
                try
                {
                    var htmlResult = _client.GetStringAsync(url).Result;
                    System.IO.File.WriteAllText(path, htmlResult);
                }
                catch(HttpRequestException ex)
                {
                    //TODO: Log
                    return null;
                }
                catch(Exception ex)
                {
                    return null;
                }
            }

            return await System.IO.File.ReadAllTextAsync(path);
        }
    }
}
