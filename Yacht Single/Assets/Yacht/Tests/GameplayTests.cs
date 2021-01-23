using NUnit.Framework;
using UnityEngine;
using Yacht.Gameplay;

namespace Yacht.Tests
{
	public class GameplayTests
	{
		[Test]
		public void Singleplay()
		{
			Player player = new Player();

			for (int round = 0; round < Constants.NUM_SCORES; round++)
			{
				for (int chance = 0; chance < 3; chance++)
				{
					player.RollDices(new[] {0, 1, 2, 3, 4});
					
					int result = Score.GetBest(player.GetDiceValues(), out var bestFit);

					if (bestFit == Enums.Category.YACHT)
					{
						if (player.Scoresheet.IsEmpty(Enums.Category.YACHT))
						{
							player.FillScoreSheet(Enums.Category.YACHT);
							break;
						}

						for (int i = (int)Enums.Category.SIXES; i >= (int)Enums.Category.ONES; i--)
						{
							if (player.Scoresheet.IsEmpty((Enums.Category) i))
							{
								player.FillScoreSheet((Enums.Category) i);
								break;
							}
						}
					}

					if (bestFit == Enums.Category.LARGE_STRAIGHT)
					{
						player.FillScoreSheet(Enums.Category.YACHT);
						break;
					}

				}
			}
		}
	}
}