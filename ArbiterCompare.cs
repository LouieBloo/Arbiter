using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsBetting
{
    class ArbiterCompare
    {

        List<WebsiteScraper> allScrapers;

        public void startComparing(List<WebsiteScraper> allScrapersIn)
        {

            allScrapers = allScrapersIn;


            if(allScrapers == null)
            {
                Helper.writeError("No Scrapers found!", "ArbiterCompare");
            }
            else
            {
                //looping over every scraper, and comparing each scrapers games to one another
                int xIndex = 0;
                while(xIndex < allScrapers.Count)
                {
                    int yIndex = xIndex+1;//since we dont want to compare them twice
                    while(yIndex < allScrapers.Count)
                    {
                        findSimilarGame(xIndex, yIndex);
                        yIndex++;
                    }
                    xIndex++;
                }
            }
      
        }



        void findSimilarGame(int indexX, int indexY)
        {
            List<Game> xGames = allScrapers[indexX].getGames();
            List<Game> yGames = allScrapers[indexY].getGames();

            int xIndex = 0;

            while(xIndex < xGames.Count)
            {
                int yIndex = 0;
                while(yIndex < yGames.Count)
                {
                   
                    if(findTeamMatch(indexX,indexY,xIndex, yIndex))
                    {
                        //MainForm.logConsole("Found Same Game: " + allScrapers[indexX].getGames()[xIndex].dumpData() + allScrapers[indexY].getGames()[yIndex].dumpData());
                        MainForm.logConsole("Scraper: " + indexX + " Game: " + xIndex);
                        MainForm.logConsole("Scraper: " + indexY + " Game: " + yIndex);
                        MainForm.logConsole("-------------");

                        findArbitrage(indexX, indexY, xIndex, yIndex);

                    }
                    
                   
                    yIndex++;
                }

                xIndex++;
            }
        }

        //Will also fix any home team, away team inconsistencies
        bool findTeamMatch(int xScraper, int yScraper, int xGame, int yGame)
        {
            //need the actual game so we can swap when necessary
            Game teamOne = allScrapers[xScraper].getGames()[xGame];
            Game teamTwo = allScrapers[yScraper].getGames()[yGame];

            if (Helper.nameMatch(null, teamOne.homeTeam.name, teamTwo.homeTeam.name))//See if home teams match
            {
                if (Helper.nameMatch(null, teamOne.awayTeam.name, teamTwo.awayTeam.name))//If they do, and away team matches, we have the same game
                {
                    return true;
                }
                else if(Helper.nameMatch(null, teamOne.homeTeam.name, teamTwo.awayTeam.name))//watching for weird behavior
                {
                    Helper.writeError("Found Weird Naming occurane: HomeName" + teamOne.homeTeam.name + " - " + teamOne.awayTeam.name + " --- AwayName" + teamTwo.homeTeam + " - " + teamTwo.awayTeam, "ArbiterCompare");
                }
                else if (Helper.nameMatch(null, teamOne.awayTeam.name, teamTwo.homeTeam.name))
                {
                    Helper.writeError("Found Weird Naming occurane: HomeName" + teamOne.homeTeam.name + " - " + teamOne.awayTeam.name + " --- AwayName" + teamTwo.homeTeam + " - " + teamTwo.awayTeam, "ArbiterCompare");
                }

                return false;
            }
            else if(Helper.nameMatch(null, teamOne.homeTeam.name, teamTwo.awayTeam.name))
            {
                if (Helper.nameMatch(null, teamOne.awayTeam.name, teamTwo.homeTeam.name))//If they do, and away team matches, we have the same game
                {
                    //swap the names to maintain consistency
                    Team holder = allScrapers[yScraper].getGames()[yGame].awayTeam;
                    allScrapers[yScraper].getGames()[yGame].awayTeam = allScrapers[yScraper].getGames()[yGame].homeTeam;
                    allScrapers[yScraper].getGames()[yGame].homeTeam = holder;
                    return true;
                }
                else if (Helper.nameMatch(null, teamOne.awayTeam.name, teamTwo.awayTeam.name))//watching for weird behavior
                {
                    Helper.writeError("Found Weird Naming occurane: HomeName" + teamOne.homeTeam.name + " - " + teamOne.awayTeam.name + " --- AwayName" + teamTwo.homeTeam + " - " + teamTwo.awayTeam, "ArbiterCompare");
                }

                return false;
            }
            else { 
                //Helper.writeError("Found Weird Naming occurane: HomeName" + teamOne.homeTeam.name + " - " + teamOne.awayTeam.name + " --- AwayName" + teamTwo.homeTeam + " - " + teamTwo.awayTeam, "ArbiterCompare");
                

                return false;
            }

            
        }

        

        void findArbitrage(int xScraper, int yScraper, int xGame, int yGame)
        {

            Team scraperOneHomeTeam = allScrapers[xScraper].getGames()[xGame].homeTeam;
            Team scraperOneAwayTeam = allScrapers[xScraper].getGames()[xGame].awayTeam;
            Team scraperTwoHomeTeam = allScrapers[yScraper].getGames()[yGame].homeTeam;
            Team scraperTwoAwayTeam = allScrapers[yScraper].getGames()[yGame].awayTeam;

            Team underdog;
            Team favorite;
            float percentMadeOnArb = 0;


            bool foundArb = false;       

            if (scraperOneHomeTeam.moneyLine < 0 && scraperOneAwayTeam.moneyLine < 0)
            {
                return;
            }
            if (scraperTwoHomeTeam.moneyLine < 0 && scraperTwoAwayTeam.moneyLine < 0)
            {
                return;
            }

            if (scraperOneHomeTeam.moneyLine < 0)
            {
                if(Math.Abs(scraperOneHomeTeam.moneyLine) < scraperTwoAwayTeam.moneyLine && scraperTwoAwayTeam.moneyLine > 0)
                {
                    underdog = scraperTwoAwayTeam;
                    favorite = scraperOneHomeTeam;

                    float underdogOdds = Helper.calculateDecimalOdds(underdog.moneyLine);
                    float favoriteOdds = Helper.calculateDecimalOdds(favorite.moneyLine);

                    percentMadeOnArb = 100 * (((favoriteOdds - 1) - (favoriteOdds / underdogOdds)) / (1 + (favoriteOdds / underdogOdds)));

                    MainForm.logConsole("!!!!!!!!!!!!!!!!ARB!!!!!!!!!!!!!!");
                    MainForm.logConsole(allScrapers[xScraper].getGames()[xGame].dumpData());
                    MainForm.logConsole(allScrapers[yScraper].getGames()[yGame].dumpData());
                    MainForm.logConsole("!1homeTeam < 0, awayTeam!");

                    foundArb = true;
                }
            }
            else if(scraperOneAwayTeam.moneyLine < 0 && scraperOneAwayTeam.moneyLine != 0)
            {
                if (Math.Abs(scraperOneAwayTeam.moneyLine) < scraperTwoHomeTeam.moneyLine && scraperTwoHomeTeam.moneyLine > 0)
                {

                    underdog = scraperTwoHomeTeam;
                    favorite = scraperOneAwayTeam;

                    float underdogOdds = Helper.calculateDecimalOdds(underdog.moneyLine);
                    float favoriteOdds = Helper.calculateDecimalOdds(favorite.moneyLine);

                    percentMadeOnArb = 100 * (((favoriteOdds - 1) - (favoriteOdds / underdogOdds)) / (1 + (favoriteOdds / underdogOdds)));

                    MainForm.logConsole("!!!!!!!!!!!!!!!!ARB!!!!!!!!!!!!!!");
                    MainForm.logConsole(allScrapers[xScraper].getGames()[xGame].dumpData());
                    MainForm.logConsole(allScrapers[yScraper].getGames()[yGame].dumpData());
                    MainForm.logConsole("!1awayTeam < 0!");
                    foundArb = true;
                }
            }
            else if (scraperTwoHomeTeam.moneyLine < 0 && scraperTwoHomeTeam.moneyLine != 0)
            {
                if (Math.Abs(scraperTwoHomeTeam.moneyLine) < scraperOneAwayTeam.moneyLine && scraperOneAwayTeam.moneyLine > 0)
                {

                    underdog = scraperOneAwayTeam;
                    favorite = scraperTwoHomeTeam;

                    float underdogOdds = Helper.calculateDecimalOdds(underdog.moneyLine);
                    float favoriteOdds = Helper.calculateDecimalOdds(favorite.moneyLine);

                    percentMadeOnArb = 100 * (((favoriteOdds - 1) - (favoriteOdds / underdogOdds)) / (1 + (favoriteOdds / underdogOdds)));

                    MainForm.logConsole("!!!!!!!!!!!!!!!!ARB!!!!!!!!!!!!!!");
                    MainForm.logConsole(allScrapers[xScraper].getGames()[xGame].dumpData());
                    MainForm.logConsole(allScrapers[yScraper].getGames()[yGame].dumpData());
                    MainForm.logConsole("!2home < 0!");
                    foundArb = true;
                }
            }
            else if (scraperTwoAwayTeam.moneyLine < 0 && scraperTwoAwayTeam.moneyLine != 0)
            {
                if (Math.Abs(scraperTwoAwayTeam.moneyLine) < scraperOneHomeTeam.moneyLine && scraperOneHomeTeam.moneyLine > 0)
                {
                    underdog = scraperOneHomeTeam;
                    favorite = scraperTwoAwayTeam;

                    float underdogOdds = Helper.calculateDecimalOdds(underdog.moneyLine);
                    float favoriteOdds = Helper.calculateDecimalOdds(favorite.moneyLine);

                    percentMadeOnArb = 100 * (((favoriteOdds - 1) - (favoriteOdds / underdogOdds)) / (1 + (favoriteOdds / underdogOdds)));

                    MainForm.logConsole("!!!!!!!!!!!!!!!!ARB!!!!!!!!!!!!!!");
                    MainForm.logConsole(allScrapers[xScraper].getGames()[xGame].dumpData());
                    MainForm.logConsole(allScrapers[yScraper].getGames()[yGame].dumpData());
                    MainForm.logConsole("!2away < 0!");
                    foundArb = true;
                }
            }
            else
            {
                return;
            }


            if(foundArb)
            {
                Game game1 = allScrapers[xScraper].getGames()[xGame];
                Game game2 = allScrapers[yScraper].getGames()[yGame];
                //Helper.writeArbitrage("Scraper: " + xScraper + " Game: " + xGame + Environment.NewLine + "Scraper: " + yScraper + " Game: " + yGame, xScraper + " " + yScraper + " " + xGame + " " + yGame + Environment.NewLine + allScrapers[xScraper].getGames()[xGame].dumpData() + Environment.NewLine + allScrapers[yScraper].getGames()[yGame].dumpData());
                Helper.writeArbitrage("Scraper" + xScraper + "Game" + xGame + "..Scraper" + yScraper + "Game" + yGame, xScraper + " " + yScraper + " " + xGame + " " + yGame + Environment.NewLine + allScrapers[xScraper].getGames()[xGame].dumpData() + Environment.NewLine + allScrapers[yScraper].getGames()[yGame].dumpData());

                MainClass.GlobalArbitrage.webCom.sendArbitrageToServer(game1,game2,percentMadeOnArb);
                allScrapers[xScraper].getGames()[xGame].hasBeenSentToServer = true;
                allScrapers[yScraper].getGames()[yGame].hasBeenSentToServer = true;
                //WebSiteCommunication web = new WebSiteCommunication();
                //web.sendArbitrageToServer(game1,game2,percentMadeOnArb);


            }
        }


        void createArbitrage(int xScraper, int xGame, int yScraper, int yGame)
        {
            string finalFilename = "";
            string finalOutput = "";

            finalFilename += "";
            
        }


        float getProfitPercent()
        {
            return 0;
        }


    }
}
