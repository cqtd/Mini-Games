using System;
using System.Collections.Generic;
using System.Text;

namespace Yacht.Game.Model
{
    class Dice
    {
        private int m_value;
        private readonly Random random;
        private static int diceCount;

        public Dice()
        {
            random = new Random(DateTime.Now.Millisecond * 177 % 1000 * diceCount + DateTime.Now.Millisecond);
            diceCount++;
        }

        public int GetValue()
        {
            return m_value;
        }

        public void SetValue(int value)
        {
            m_value = value;
        }

        public void Roll()
        {
            m_value = new Random().Next(1, 6);
        }
    }
}
