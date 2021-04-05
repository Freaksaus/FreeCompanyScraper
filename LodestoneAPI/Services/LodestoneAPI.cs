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
        private const int MAX_TOTAL_RESULTS = 1000;

        private readonly List<string> _searchList;

        public LodestoneAPI(ILodestoneParser lodestoneParser, IOptions<APIOptions> options)
        {
            _lodestoneParser = lodestoneParser ?? throw new ArgumentException(nameof(lodestoneParser));
            _options = options.Value ?? throw new ArgumentException(nameof(options));

            _client = new HttpClient();
            _client.DefaultRequestHeaders.Add("User-Agent", "Free Company Scraper");

            _searchList = new List<string>();
            for (char c = 'a'; c <= 'z'; c++)
            {
                _searchList.Add(c.ToString());
            }
        }

        public async Task<Character> GetCharacter(string id)
        {
            var result = await GetCharacterPage(id);
            return result;
        }

        public async Task<IEnumerable<FreeCompanyEntry>> GetFreeCompanies(string serverName)
        {
            int page = 1;
            var searchResult = await GetFreeCompaniesPerPage(serverName, "", page);

            return searchResult.FreeCompanies;
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

        private async Task<FreeCompanySearchResult> GetFreeCompaniesPerPage(string serverName, string searchText, int page)
        {
            var totalResult = new FreeCompanySearchResult()
            {
                FreeCompanies = new List<FreeCompanyEntry>(),
                Page = page,
                SearchText = searchText,
                TotalResults = 0
            };

            Console.WriteLine($"Search companies with text: {searchText} and page {page}");

            var filename = $"Companies/{serverName}/{searchText}-{page}.html";
            var url = $"https://na.finalfantasyxiv.com/lodestone/freecompany/?q={searchText}&worldname={serverName}&character_count=&activetime=&join=&house=&order=&page={page}";

            Models.FreeCompanySearchResult pageResult = null;
            int downloadCount = 0;
            while(pageResult == null && downloadCount < 3)
            {
                var html = await GetHtml(url, filename);
                downloadCount++;

                if (string.IsNullOrWhiteSpace(html))
                {
                    return totalResult;
                }

                pageResult = await _lodestoneParser.ParseFreeCompanySearchPage(html, searchText, page);
                if (pageResult == null)
                {
                    System.IO.File.Delete(System.IO.Path.Combine(_options.CacheDirectory, filename));
                    System.Threading.Thread.Sleep(5000);
                }
            }

            if(pageResult == null)
            {
                return totalResult;
            }

            if (pageResult.TotalResults >= MAX_TOTAL_RESULTS)
            {
                foreach (var searchItem in _searchList)
                {
                    var tempPage = 1;
                    var tempResult = await GetFreeCompaniesPerPage(serverName, $"{searchText}{searchItem}", tempPage);
                    var totalPages = Math.Ceiling((decimal)tempResult.TotalResults / MAX_RESULTS_PER_PAGE);

                    while (tempResult != null && tempResult.FreeCompanies.Count == MAX_RESULTS_PER_PAGE && tempPage < totalPages && tempPage < MAX_PAGES)
                    {
                        tempPage++;
                        tempResult = await GetFreeCompaniesPerPage(serverName, $"{searchText}{searchItem}", tempPage);
                        totalResult.FreeCompanies.AddRange(tempResult.FreeCompanies);
                    }
                }
            }
            else
            {
                totalResult.FreeCompanies.AddRange(pageResult.FreeCompanies);
                totalResult.TotalResults += pageResult.TotalResults;
            }

            return totalResult;
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
                System.Threading.Thread.Sleep(2000);
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
