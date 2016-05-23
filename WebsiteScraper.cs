using System;
using System.Collections.Generic;

namespace SportsBetting
{
	public interface WebsiteScraper
	{
		List<Game> getGames();
        void removeGame(int index);
        void startPull(string sport);
        void scrapeData(string sport);
        bool isQueued();
        void setQueued(bool queued);
        string getName();
        

	}
}

