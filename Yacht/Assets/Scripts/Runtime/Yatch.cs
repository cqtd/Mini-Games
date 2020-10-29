using System.Collections.Generic;

namespace CQ.MiniGames
{
	public class Yatch
	{
		public enum EYatchMode
		{
			Classic,
		}

		EYatchMode mode;
		readonly List<Player> players = new List<Player>();
		
		
		public class Builder
		{
			Yatch instance;
			
			public Builder()
			{
				instance = new Yatch();
			}

			public Builder AddPlayer(Player player)
			{
				instance.players.Add(player);
				return this;
			}

			public Builder SetMode(EYatchMode mode)
			{
				instance.mode = mode;
				return this;
			}
			
			
			public Yatch Build()
			{
				Yatch inst = instance;
				instance = null;
				
				return inst;
			}
		}
	}

	public class Player
	{
		
	}
}