using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Yacht.Gameplay;

public class ScoreValidateTest
{
    // A Test behaves as an ordinary method
    [Test]
    public void NumericScoreTestSimplePasses()
    {
        List<int> dices = new List<int>()
        {
            1, 1, 1, 2, 2
        };

        int result = Scoresheet.GetNumeric(dices, 1);
        Assert.IsTrue(result == 3);
    }
    
    [Test]
    public void SmallStrightTest()
    {
        int[][] cases = new int[][]
        {
            new[] {1, 1, 1, 1, 1},
            new[] {1, 2, 3, 6, 1},
            new[] {1, 2, 3, 2, 2},
            new[] {2, 3, 1, 5, 6},
        };

        foreach (int[] c in cases)
        {
            int result = Scoresheet.GetSmallStraight(c.ToList());
            Assert.IsTrue(result == 0);
        }

        cases = new int[][]
        {
            new[] {1, 2, 3, 4, 1},
            new[] {1, 2, 3, 4, 2},
            new[] {1, 2, 3, 4, 3},
            new[] {1, 2, 3, 4, 4},
            new[] {1, 2, 3, 4, 5},
            new[] {1, 2, 3, 4, 6},
            new[] {2, 3, 4, 5, 1},
            new[] {2, 3, 4, 5, 2},
            new[] {2, 3, 4, 5, 3},
            new[] {2, 3, 4, 5, 4},
            new[] {2, 3, 4, 5, 5},
            new[] {2, 3, 4, 5, 6},
            new[] {3, 4, 5, 6, 1},
            new[] {3, 4, 5, 6, 2},
            new[] {3, 4, 5, 6, 3},
            new[] {3, 4, 5, 6, 4},
            new[] {3, 4, 5, 6, 5},
        };

        foreach (int[] c in cases)
        {
            int result = Scoresheet.GetSmallStraight(c.ToList());
            Assert.IsTrue(result == 15);
        }
    }

    public void LargeStraight()
    {
        int[][] cases = new int[][]
        {
            new[] {1, 1, 1, 1, 1},
            new[] {1, 2, 3, 4, 1},
            new[] {1, 2, 3, 2, 2},
            new[] {2, 3, 1, 5, 6},
        };

        foreach (int[] c in cases)
        {
            int result = Scoresheet.GetLargeStraight(c.ToList());
            Assert.IsTrue(result == 0);
        }

        cases = new int[][]
        {
            new[] {1, 2, 3, 4, 5},
            new[] {2, 3, 4, 5, 6},
        };

        foreach (int[] c in cases)
        {
            int result = Scoresheet.GetLargeStraight(c.ToList());
            Assert.IsTrue(result == 30);
        }
    }
}
