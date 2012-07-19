using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Threading;

namespace cardGame
{
    class gameThread
    {
        private ManualResetEvent doneEvent;
        private bool shuffle;

        private cardDeck hand1 = new cardDeck();
        private cardDeck hand2 = new cardDeck();

        private cardDeck pile1 = new cardDeck();
        private cardDeck pile2 = new cardDeck();

        private int warCount1 = 0;
        private int battleCount1 = 0;
        private int warCount2 = 0;
        private int battleCount2 = 0;
        private int handReload1 = 0;
        private int handReload2 = 0;

        private void setup()
        {
            cardDeck deck = new cardDeck();
            deck.Init();
            deck.Shuffle();

            for (int i = 0; i < deck.Size - 1; i++)
            {
                hand1.Add(deck[i]);
                hand2.Add(deck[++i]);
            }
        }

        public gameThread(bool shuffle, ManualResetEvent doneEvent)
        {
            this.shuffle = shuffle;
            this.doneEvent = doneEvent;

        }

        public void run(Object threadContext)
        {
            KeyValuePair<int, int> card1, card2;
            setup();
            while ((hand1.Size + pile1.Size) > 0 && (hand2.Size + pile2.Size) > 0)
            {
                card1 = hand1.Next();
                card2 = hand2.Next();

                if(card1.Value == card2.Value)
                {
                    ArrayList pot = new ArrayList();
                    pot.Add(card1);
                    pot.Add(card2);

                    do
                    {
                        int count = (hand1.Size < 3 ? hand1.Size : 3);
                        
                        for (int i = 0; i < count; i++)
                        {
                            pot.Add(hand1.Next());
                        }
                        if (hand1.Size == 0 && pile1.Size > 0)
                        {
                            hand1.Rebuild(pile1.Dump());
                            if (shuffle && hand1.Size > 0)
                                hand1.Shuffle();
                            handReload1++;
                        }
                        if(hand1.Size > 0)
                            card1 = hand1.Next();

                        count = (hand2.Size < 3 ? hand2.Size : 3);
                        for (int i = 0; i < count; i++)
                        {
                            pot.Add(hand2.Next());
                        }
                        if (hand2.Size == 0 && pile2.Size > 0)
                        {
                            hand2.Rebuild(pile2.Dump());
                            if (shuffle && hand2.Size > 0)
                                hand2.Shuffle();
                            handReload2++;
                        }
                        if(hand2.Size > 0)
                            card2 = hand2.Next();

                    }while(card1.Value != card2.Value);

                    if (card1.Value > card2.Value)
                    {
                        pile1.Add(card1);
                        pile1.Add(card2);
                        for (int i = 0; i < pot.Count; i++)
                        {
                            pile1.Add((KeyValuePair<int, int>)pot[i]);
                        }
                        warCount1++;
                    }
                    else
                    {
                        pile2.Add(card1);
                        pile2.Add(card2);
                        for (int i = 0; i < pot.Count; i++)
                        {
                            pile2.Add((KeyValuePair<int, int>)pot[i]);
                        }
                        warCount2++;
                    }

                }
                else if (card1.Value > card2.Value)
                {
                    pile1.Add(card1);
                    pile1.Add(card2);
                    battleCount1++;
                }
                else
                {
                    pile2.Add(card1);
                    pile2.Add(card2);
                    battleCount2++;
                }

                if (hand1.Size == 0 && pile1.Size > 0)
                {
                    hand1.Rebuild(pile1.Dump());
                    if (shuffle && hand1.Size > 0)
                        hand1.Shuffle();
                    handReload1++;
                }

                if (hand2.Size == 0 && pile2.Size > 0)
                {
                    hand2.Rebuild(pile2.Dump());
                    if (shuffle && hand2.Size > 0)
                        hand2.Shuffle();
                    handReload2++;
                }
            }
            System.Console.WriteLine("Shuffled: " + (shuffle ? "True" : "False") + " Battles - Hand1: " + battleCount1.ToString() + " Hand2: " + battleCount2.ToString() + " Wars - Hand1: " + warCount1.ToString() + " Hand2: " + warCount2.ToString() + " Reloads - Hand1: " + handReload1.ToString() + " Hand2: " + handReload2.ToString());
            doneEvent.Set();
        }
    }
}
