using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace LodestoneAPI.Services
{
    public class LodestoneParser : ILodestoneParser
    {
        private System.Text.RegularExpressions.Regex _dateTimeRegex = new System.Text.RegularExpressions.Regex(@"ldst_strftime\(([0-9]+)");

        public Task<List<Models.FreeCompanyMemberEntry>> ParseFreeCompanyMemberPage(string html, string freeCompanyId)
        {
            var document = new HtmlDocument();
            document.LoadHtml(html);

            var result = new List<Models.FreeCompanyMemberEntry>();
            var entries = document.DocumentNode.SelectNodes("//li[@class='entry']");
            if (entries == null || !entries.Any())
            {
                return Task.FromResult(result);
            }

            foreach (var entry in entries)
            {
                var linkElement = entry.SelectSingleNode("a");
                var memberCharacterId = GetIdFromNode(linkElement);

                var nameElement = linkElement.SelectSingleNode("div/div/p[@class='entry__name']");
                var name = System.Web.HttpUtility.HtmlDecode(nameElement.InnerText);

                var model = new Models.FreeCompanyMemberEntry()
                {
                    Id = memberCharacterId,
                    Name = name,
                    FreeCompanyId = freeCompanyId,
                };

                result.Add(model);
            }

            return Task.FromResult(result);
        }

        public Task<Models.FreeCompanySearchResult> ParseFreeCompanySearchPage(string html, string searchText, int page)
        {
            var document = new HtmlDocument();
            document.LoadHtml(html);

            var result = new Models.FreeCompanySearchResult()
            {
                Page = page,
                SearchText = searchText,
                FreeCompanies = new List<Models.FreeCompanyEntry>(),
                TotalResults = 0
            };

            var emptyElement = document.DocumentNode.SelectSingleNode("//p[@class='parts__zero']");
            if(emptyElement != null)
            {
                result = null;
                return Task.FromResult(result);
            }

            var entries = document.DocumentNode.SelectNodes("//div[@class='entry']");
            if (entries == null || !entries.Any())
            {
                result = null;
                return Task.FromResult(result);
            }

            var totalResultsNode = document.DocumentNode.SelectSingleNode("//div[@class='parts__total']");
            if (totalResultsNode == null)
            {
                result = null;
                return Task.FromResult(result);
            }
            var totalResults = Convert.ToInt32(totalResultsNode.InnerText.Replace("Total", ""));

            result = new Models.FreeCompanySearchResult()
            {
                SearchText = searchText,
                Page = page,
                FreeCompanies = new List<Models.FreeCompanyEntry>(),
                TotalResults = totalResults
            };

            foreach (var entry in entries)
            {
                var linkNode = entry.SelectSingleNode("a");
                var freeCompanyId = GetIdFromNode(linkNode);

                var nameElement = linkNode.SelectSingleNode("div/div/p[@class='entry__name']");
                var name = System.Web.HttpUtility.HtmlDecode(nameElement.InnerText);

                var memberCountElement = linkNode.SelectSingleNode("ul/li[@class='entry__freecompany__fc-member']");
                var memberCount = 0;

                if(memberCountElement != null)
                {
                    memberCount = Convert.ToInt32(memberCountElement.InnerHtml.Trim());
                }

                var dateCreatedElement = linkNode.SelectSingleNode("ul/li[@class='entry__freecompany__fc-day']/script");
                var dateCreated = DateTime.MinValue;

                if(dateCreatedElement != null && _dateTimeRegex.IsMatch(dateCreatedElement.InnerText))
                {
                    var timespan = _dateTimeRegex.Match(dateCreatedElement.InnerText).Groups[1].Value;
                    dateCreated = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(timespan)).DateTime;
                }

                var model = new Models.FreeCompanyEntry()
                {
                    DateCreated = dateCreated,
                    Id = freeCompanyId,
                    MemberCount = memberCount,
                    Name = name,
                };

                result.FreeCompanies.Add(model);
            }

            return Task.FromResult(result);
        }

        public Task<Models.FreeCompany> ParseFreeCompanyPage(string html, string freeCompanyId)
        {
            var document = new HtmlDocument();
            document.LoadHtml(html);

            var dateCreated = DateTime.MinValue;
            var dateCreatedLabelNode = document.DocumentNode.SelectSingleNode("//h3[@class='heading--lead' and text() = 'Formed']");
            if (dateCreatedLabelNode != null && dateCreatedLabelNode.NextSibling.NextSibling != null)
            {
                var dateCreatedElement = dateCreatedLabelNode.NextSibling.NextSibling.SelectSingleNode("script");
                if (dateCreatedElement != null && _dateTimeRegex.IsMatch(dateCreatedElement.InnerText))
                {
                    var timespan = _dateTimeRegex.Match(dateCreatedElement.InnerText).Groups[1].Value;
                    dateCreated = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(timespan)).DateTime;
                }
            }


            var estateAddress = "";
            var estateAddressNode = document.DocumentNode.SelectSingleNode("//p[@class='freecompany__estate__text']");
            if (estateAddressNode != null)
            {
                estateAddress = System.Web.HttpUtility.HtmlDecode(estateAddressNode.InnerText);
            }

            var estateName = "";
            var estateNameNode = document.DocumentNode.SelectSingleNode("//p[@class='freecompany__estate__name']");
            if(estateNameNode != null)
            {
                estateName = System.Web.HttpUtility.HtmlDecode(estateNameNode.InnerText);
            }

            var memberCount = 0;
            var memberCountLabelNode = document.DocumentNode.SelectSingleNode("//h3[@class='heading--lead' and text() = 'Active Members']");
            if(memberCountLabelNode != null && memberCountLabelNode.NextSibling.NextSibling != null)
            {
                memberCount = Convert.ToInt32(memberCountLabelNode.NextSibling.NextSibling.InnerText);
            }

            var nameNode = document.DocumentNode.SelectSingleNode("//p[@class='entry__freecompany__name']");
            var name = System.Web.HttpUtility.HtmlDecode(nameNode.InnerText);

            var tagNode = document.DocumentNode.SelectSingleNode("//p[@class='freecompany__text freecompany__text__tag']");
            var tag = System.Web.HttpUtility.HtmlDecode(tagNode.InnerText.Replace("&laquo;", "").Replace("&raquo;", ""));

            var result = new Models.FreeCompany()
            {
                DateCreated = dateCreated,
                EstateAddress = estateAddress,
                EstateName = estateName,
                MemberCount = memberCount,
                Id = freeCompanyId,
                Name = name,
                Tag = tag
            };

            return Task.FromResult(result);
        }

        public Task<Models.Character> ParseCharacterPage(string html, string characterId)
        {
            var document = new HtmlDocument();
            document.LoadHtml(html);

            var entry = document.DocumentNode.SelectSingleNode("//div[@id='character']");
            if (entry == null)
            {
                return null;
            }

            var nameElement = document.DocumentNode.SelectSingleNode("//p[@class='frame__chara__name']");
            var name = System.Web.HttpUtility.HtmlDecode(nameElement.InnerText.Trim());

            var characterElement = document.DocumentNode.SelectSingleNode("//p[@class='character-block__name']");
            var characterElements = System.Web.HttpUtility.HtmlDecode(characterElement.InnerText).Split("/");
            var raceClanElement = characterElements[0].Trim();
            var genderElement = characterElements[1].Trim();

            var (race, clan) = ConvertToRaceClan(raceClanElement);
            var gender = ConvertToGender(genderElement);
            

            var freeCompanyLinkNode = document.DocumentNode.SelectSingleNode("//div[@class='character__freecompany__name']/h4/a");
            var freeCompanyId = GetIdFromNode(freeCompanyLinkNode);

            var characterClassNodes = document.DocumentNode.SelectNodes("//div[@class='character__level__list']/ul/li");
            var highestLevel = characterClassNodes?.Where(c => c.InnerText != "-").Max(c => Convert.ToInt32(c.InnerText)) ?? 0; 

            var result = new Models.Character()
            {
                Id = characterId,
                Name = name,
                Clan = clan,
                FreeCompanyId = freeCompanyId,
                Gender = gender,
                HighestLevel = highestLevel,
                Race = race,
            };

            return Task.FromResult(result);
        }

        private string GetIdFromNode(HtmlNode node)
        {
            if(node == null)
            {
                return null;
            }

            var url = node.Attributes["href"].Value;
            var parts = url.Split(@"/");
            var id = parts[parts.Length - 2].Trim();

            return id;
        }

        private Models.Gender ConvertToGender(string gender)
        {
            switch(gender)
            {
                case "♀":
                    return Models.Gender.Female;
                case "♂":
                    return Models.Gender.Male;
                default:
                    throw new ArgumentException(nameof(gender));
            }
        }

        private (Models.Race, Models.Clan) ConvertToRaceClan(string clan)
        {
            switch (clan)
            {
                case "LalafellDunesfolk":
                    return (Models.Race.Lalafell, Models.Clan.Dunesfolk);
                case "ElezenDuskwight":
                    return (Models.Race.Elezen, Models.Clan.Duskwight);
                case "HrothgarHelions":
                    return (Models.Race.Hrothgar, Models.Clan.Helions);
                case "RoegadynHellsguard":
                    return (Models.Race.Roegadyn, Models.Clan.Hellsguard);
                case "HyurHighlander":
                    return (Models.Race.Hyur, Models.Clan.Highlander);
                case "Miqo'teKeeper of the Moon":
                    return (Models.Race.Miqote, Models.Clan.KeeperOfTheMoon);
                case "HyurMidlander":
                    return (Models.Race.Hyur, Models.Clan.Midlander);
                case "LalafellPlainsfolk":
                    return (Models.Race.Lalafell, Models.Clan.Plainsfolk);
                case "Au RaRaen":
                    return (Models.Race.AuRa, Models.Clan.Raen);
                case "VieraRava":
                    return (Models.Race.Viera, Models.Clan.Rava);
                case "RoegadynSea Wolf":
                    return (Models.Race.Roegadyn, Models.Clan.SeaWolves);
                case "Miqo'teSeeker of the Sun":
                    return (Models.Race.Miqote, Models.Clan.SeekerOfTheSun);
                case "HrothgarThe Lost":
                    return (Models.Race.Hrothgar, Models.Clan.TheLost);
                case "VieraVeena":
                    return (Models.Race.Viera, Models.Clan.Veena);
                case "ElezenWildwood":
                    return (Models.Race.Elezen, Models.Clan.Wildwood);
                case "Au RaXaela":
                    return (Models.Race.AuRa, Models.Clan.Xaela);
                default:
                    throw new ArgumentException(nameof(clan));
            }
        }


        private Models.Race ConvertToRace(string race)
        {
            switch (race)
            {
                case "AuRa":
                    return Models.Race.AuRa;
                case "Elezen":
                    return Models.Race.Elezen;
                case "Hrothgar":
                    return Models.Race.Hrothgar;
                case "Hyur":
                    return Models.Race.Hyur;
                case "Lalafell":
                    return Models.Race.Lalafell;
                case "Miqo'te":
                    return Models.Race.Miqote;
                case "Roegadyn":
                    return Models.Race.Roegadyn;
                case "Viera":
                    return Models.Race.Viera;
                default:
                    throw new ArgumentException(nameof(race));
            }
        }
    }
}
