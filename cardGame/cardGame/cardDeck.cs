using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;

namespace cardGame
{
    class cardDeck
    {
        private ArrayList cards = new ArrayList();
        private Random rand;

        public cardDeck(Random rand)
        {
            this.rand = rand;
        }

        public void Init()
        {
            for (int i = 1; i <= 4; i++)
            {
                for (int j = 2; j <= 14; j++)
                {
                    cards.Add(new KeyValuePair<int, int>(i, j));
                }
            }
            cards.Add(new KeyValuePair<int, int>(0, 15));
            cards.Add(new KeyValuePair<int, int>(0, 15));
        }

        public void Shuffle()
        {
            KeyValuePair<int, int> temp;
            int index1, index2;
            for (int i = 0; i < (cards.Count * 2); i++)
            {
                index1 = rand.Next(0, cards.Count);
                index2 = rand.Next(0, cards.Count);
                temp = (KeyValuePair<int, int>) cards[index1];
                cards[index1] = cards[index2];
                cards[index2] = temp;
            }
        }

        public void Add(KeyValuePair<int, int> card)
        {
            cards.Add(card);
        }

        public int Size
        {
            get
            {
                return cards.Count;
            }
        }

        public KeyValuePair<int, int> Next()
        {
            KeyValuePair<int, int> next = (KeyValuePair<int, int>) cards[cards.Count - 1];
            cards.RemoveAt(cards.Count - 1);
            return next;
        }

        public KeyValuePair<int, int> this [int index]
        {
            get
            {
                return (KeyValuePair<int, int>)cards[index];
            }
        }

        public void Rebuild(ArrayList pile)
        {
            cards = (ArrayList) pile.Clone();
            cards.Reverse();
        }

        public ArrayList Dump()
        {
            ArrayList temp = cards;
            cards.Clear();
            return temp;
        }
    }
}
