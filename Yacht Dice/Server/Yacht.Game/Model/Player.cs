using System;
using System.Collections.Generic;
using Yacht.Game.Common;

namespace Yacht.Game.Model
{
    class Player
    {
        private ScoreSheet m_scoreSheet;
        private readonly List<Dice> m_dices = new List<Dice>(Constants.NUM_DICES);
        private readonly List<int> m_scores = new List<int>(Constants.NUM_CATEGORIES);

        private int m_numReroll = 0;

        public void Initialize()
        {
            
        }

        public ScoreSheet GetScoreSheet()
        {
            throw new NotImplementedException();
        }

        public List<Dice> GetDiceValues()
        {
            throw new NotImplementedException();
        }

        public List<int> GetScores()
        {
            throw new NotImplementedException();
        }

        public void SetDiceValues(List<int> diceValues)
        {
            throw new NotImplementedException();
        }

        public void RollDices(IEnumerable<int> diceIndices)
        {
            throw new NotImplementedException();
        }

        public void CalculateScores()
        {
            throw new NotImplementedException();
        }

        public void FillScoreSheet(Enums.ECategory category)
        {
            throw new NotImplementedException();
        }

        public event Action processNextPlayerCallback;
    }
}