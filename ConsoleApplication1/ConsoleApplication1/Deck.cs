using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VincentFantini {

    class Deck {
        // First, we create the playing cards and assign an integer value to each card based on their value in the Pontifex encryption algorithm.
        // The cards are sorted in their numerical values, and the suits are arranged in bridge order (Clubs, then Diamonds, then Hearts, then Spades).
        // At the end of the deck are the Small Joker (SJ) and the Large Joker (LJ).  Both Jokers have the same numerical value (53).

        int sjLocation = 0; // This variable will contain the location of the Small Joker card.
        int ljLocation = 0; // This variable will contain the location of the Large Joker card.

        enum Cards {
            LJ,
            AceC, TwoC, ThreeC, FourC, FiveC, SixC, SevenC, EightC, NineC, TenC, JackC, QueenC, KingC,
            AceD, TwoD, ThreeD, FourD, FiveD, SixD, SevenD, EightD, NineD, TenD, JackD, QueenD, KingD,
            AceH, TwoH, ThreeH, FourH, FiveH, SixH, SevenH, EightH, NineH, TenH, JackH, QueenH, KingH,
            AceS, TwoS, ThreeS, FourS, FiveS, SixS, SevenS, EightS, NineS, TenS, JackS, QueenS, KingS,
            SJ,
        }

        // Next, we create a deck of all 54 cards by creating an array.
        ArrayList deck = new ArrayList() {
            Cards.AceC, Cards.TwoC, Cards.ThreeC, Cards.FourC, Cards.FiveC, Cards.SixC, Cards.SevenC, Cards.EightC, Cards.NineC, Cards.TenC, Cards.JackC, Cards.QueenC, Cards.KingC,
            Cards.AceD, Cards.TwoD, Cards.ThreeD, Cards.FourD, Cards.FiveD, Cards.SixD, Cards.SevenD, Cards.EightD, Cards.NineD, Cards.TenD, Cards.JackD, Cards.QueenD, Cards.KingD,
            Cards.AceH, Cards.TwoH, Cards.ThreeH, Cards.FourH, Cards.FiveH, Cards.SixH, Cards.SevenH, Cards.EightH, Cards.NineH, Cards.TenH, Cards.JackH, Cards.QueenH, Cards.KingH, 
            Cards.AceS, Cards.TwoS, Cards.ThreeS, Cards.FourS, Cards.FiveS, Cards.SixS, Cards.SevenS, Cards.EightS, Cards.NineS, Cards.TenS, Cards.JackS, Cards.QueenS, Cards.KingS, 
            Cards.SJ, Cards.LJ
        };

        // Now to test to see if this actually works.  This checkDeck() method will display the entire deck from top to bottom.
        public void checkDeck() {
            for (int i = 0; i < deck.Count; i++) {
                Console.WriteLine("Card {0} is {1}; Value = {2}", i, deck[i], (int)deck[i]);
            }
            return;
        }

        public void randomizeDeck() {
            Console.Write("Would you like to shuffle the deck (y/n)?: ");
            string shuffleAnswer = Console.ReadLine();
            if (shuffleAnswer == "y") {
                int[] randomizer = new int[54];
                for (int i = 0; i < randomizer.Length; i++) {
                    randomizer[i] = i;
                }

                // I yanked this next block of code from the internet; it's the Fisher-Yates shuffle found here:  http://www.dotnetperls.com/fisher-yates-shuffle
                Random _random = new Random();
                var random = _random;
                for (int i = randomizer.Length; i > 1; i--) {
                    // Pick random element to swap.
                    int j = random.Next(i); // 0 <= j <= i-1
                    // Swap.
                    int tmp = randomizer[j];
                    randomizer[j] = randomizer[i - 1];
                    randomizer[i - 1] = tmp;
                }

/*              // What I want to do here is to use the Cards enumeration's underlying int values (LJ = 0, AceC = 1, etc.) as a way to randomize the Deck.
 *              // I'll use the randomized randomizer[] array as a way to create a randomized set of values between 0 and 53, and then I'll add them onto the end of the deck ArrayList.
 *              // I'll then delete the first 54 elements from the deck ArrayList, leaving only the randomized cards within the Deck itself.
 *              // NOTE:  This obviously didn't work the way I had hoped it would.  I'll have to continue working on it later.
 *              
 *              for (int i = 0; i < randomizer.Length; i++) {
                    deck.Add(randomizer[i]);
                }
*/
            }
        }

        // This method will return the actual deck ArrayList for purposes of creating the duplicate deck.  The duplicate deck's purpose
        // is to generate the same keystream numbers as the original deck; these keystream numbers are what allows the user to decipher
        // the encrypted message.
        public ArrayList deckArrayList() {
            return deck;
        }

        //This method will create a duplicate Deck instance of whatever original Deck instance is passed to it as a parameter.
        public void duplicateDeck(Deck originalDeck) {
            deck.Clear();
            ArrayList origDeck = new ArrayList();
            origDeck = originalDeck.deckArrayList();

            for (int i = 0; i < origDeck.Count; i++) {
                deck.Add(origDeck[i]);
            }
        }

        // First, we locate the Small Joker in the deck array.
        public void step1SJMove() {
            for (int i = 0; i < deck.Count; i++) {
                if ((Cards)deck[i] == Cards.SJ) {
                    sjLocation = i;
                }
            }

            // Now that we've found the Small Joker, we will now shift its position one space down within the deck.
            // If its starting position is on the bottom of the deck, it will be shifted to the position immediately after the first card in the deck.  We'll check for this first...
            if (sjLocation == deck.Count - 1) {
                deck.RemoveAt(sjLocation);
                deck.Insert(1, Cards.SJ);
                sjLocation = 1;
            }
            else {
                deck.RemoveAt(sjLocation);
                deck.Insert(sjLocation + 1, Cards.SJ);
                sjLocation += 1;
            }
        }

        // Next, we locate the Large Joker in the deck array.
        public void step2LJMove() {
            for (int i = 0; i < deck.Count; i++) {
                if ((Cards)deck[i] == Cards.LJ) {
                    ljLocation = i;
                }
            }

            // Now that we've found the Large Joker, we will now shift its position two spaces down within the deck.
            // If its starting position is on the bottom of the deck, it will be shifted to the position immediately after the second card in the deck.
            // If the Large Joker is located one card above the bottom card in the deck, it's to be shifted to just below the top card.
            if (ljLocation == deck.Count - 1) {
				deck.RemoveAt(ljLocation);
                deck.Insert(2, Cards.LJ);
                ljLocation = 2;
            }
            else if (ljLocation == deck.Count - 2) {
				deck.RemoveAt(ljLocation);
                deck.Insert(1, Cards.LJ);
                ljLocation = 1;
            }
            else {
				deck.RemoveAt(ljLocation);
                deck.Insert(ljLocation + 2, Cards.LJ);
                ljLocation += 2;
            }
        }

        // Step 3 is where we perform the triple cut - we swap out all of the cards above the Small Joker with the cards below the Large Joker.
        // The order of each of these parts of the deck are to remain in the order that they're in.
        // To do this, we'll create three separate ArrayLists that will contain each cut of the deck.  We'll then empty out the deck ArrayList,
        // and then concatenate the three separate ArrayLists back into the deck ArrayList in their proper order.
        public void step3TripleCut() {
            // We will create two new ArrayLists to copy the portions of the deck above & below the Jokers.
            ArrayList deckAboveFJ = new ArrayList(); // This will copy the portion of the deck ABOVE the First Joker (aka the Joker closest to the top of the deck).
            ArrayList deckBelowSJ = new ArrayList(); // This will copy the portion of the deck BELOW the Second Joker (aka the Joker closest to the bottom of the deck).

            // First, we determine if the Jokers are the top & bottom cards of the deck.  If they are, then this Step 3 method is to be skipped entirely.
            if ((Cards)deck[0] == Cards.SJ && (Cards)deck[deck.Count - 1] == Cards.LJ) {
                return;
            }
            else if ((Cards)deck[0] == Cards.LJ && (Cards)deck[deck.Count - 1] == Cards.SJ) {
                return;
            }
            else {
                // Next, we figure out which Joker is closer to the top of the deck, and which Joker is closer to the bottom of the deck.
                // The following if statement executes if the Small Joker is closer to the top of the deck than the Large Joker.
                if (sjLocation < ljLocation) {
                    if (sjLocation == 0) { // If the Small Joker is the top card of the deck, then nothing is to be done for this part of Step 3.
                        // Console.WriteLine("The Small Joker is on the top of the deck."); 
                    }
                    else if (sjLocation > 0) {
                        for (int i = 0; i < deck.Count && (int)deck[i] != 53; i++) {
                            deckAboveFJ.Add(deck[i]);
                        }
                    }
                    else {
                        Console.WriteLine("Error: It broke!");
                    }

                    if (ljLocation == deck.Count - 1) { // If the Large Joker is the bottom card of the deck, then nothing is to be done for this part of Step 3.
                        // Console.WriteLine("The Large Joker is on the bottom of the deck.");
                    }
                    else if (ljLocation < deck.Count - 1) {
                        for (int i = ljLocation + 1; i < deck.Count && (int)deck[i] != 0; i++) {
                            deckBelowSJ.Add(deck[i]);
                        }
                    }
                    else {
                        Console.WriteLine("Error: It broke!");
                    }
                }

                // The following if statement executes if the Large Joker is closer to the top of the deck than the Small Joker.
                else if (sjLocation > ljLocation) {
                    if (ljLocation == 0) { // If the Large Joker is the top card of the deck, then nothing is to be done for this part of Step 3.
                        // Console.WriteLine("The Large Joker is on the top of the deck.");
                    }
                    else if (ljLocation > 0) {
                        for (int i = 0; i < deck.Count && (int)deck[i] != 0; i++) {
                            deckAboveFJ.Add(deck[i]);
                        }
                    }
                    else {
                        Console.WriteLine("Error: It broke!");
                    }

                    if (sjLocation == deck.Count - 1) { // If the Small Joker is the bottom card of the deck, then nothing is to be done for this part of Step 3.
                        // Console.WriteLine("The Small Joker is on the bottom of the deck.");
                    }
                    else if (sjLocation < deck.Count - 1) {
                        for (int i = sjLocation + 1; i < deck.Count && (int)deck[i] != 53; i++) {
                            deckBelowSJ.Add(deck[i]);
                        }
                    }
                    else {
                        Console.WriteLine("Error: It broke!");
                    }
                }
                // If all else fails, we'll get an error message saying that the Jokers are located in invalid locations.
                else {
                    Console.WriteLine("Error: Small Joker (located at Card {0}) & Large Joker (located at Card {1}) locations are invalid.", sjLocation, ljLocation);
                }
                
                // Now that we've copied the cards above the First Joker to one ArrayList and the cards below the Second Joker to the other ArrayList,
                // we will now remove all of the cards above the First Joker.
                deck.RemoveRange(0, deckAboveFJ.Count);
                
                // We've found the Jokers, so we quickly determine which is the Second Joker and then remove the cards below it.
                if (sjLocation > ljLocation) {
                    sjLocation -= ljLocation; // This will update the Small Joker's locator to reflect its new location since the deck has changed in size.
                    ljLocation -= ljLocation; // This will update the Large Joker's locator to reflect its new location since the deck has changed in size.
                    deck.RemoveRange(sjLocation + 1, deckBelowSJ.Count);
                }
                else if (sjLocation < ljLocation) {
                    ljLocation -= sjLocation; // This will update the Large Joker's locator to reflect its new location since the deck has changed in size.
                    sjLocation -= sjLocation; // This will update the Small Joker's locator to reflect its new location since the deck has changed in size.
                    deck.RemoveRange(ljLocation + 1, deckBelowSJ.Count);
                }

                // Finally, we re-order the deck ArrayList so that the top & bottom cut portions of the deck are swapped.
                deck.InsertRange(0, deckBelowSJ);
                deck.InsertRange(deck.Count, deckAboveFJ);
				deckAboveFJ.Clear();
				deckBelowSJ.Clear();
            }
        }

        // For this step, we check the bottom card in the deck and check its numerical value.  This value is the number of cards we'll be counting from the top of the deck.
        // We then cut the counted cards from the bottom of the deck, and we place them just above the last card in the deck.
        public void step4CountCut() {
            int bottomCardValue = (int)deck[deck.Count - 1]; // The bottom card's value is stored in this variable.

            // Now to count & copy the cards from the top of the deck and store them in an ArrayList.
            ArrayList countCutDeck = new ArrayList(); // This will copy the cards counted from the top of the deck.
            for (int i = 0; i < bottomCardValue && i < deck.Count; i++) {
                countCutDeck.Add(deck[i]);
            }
            deck.RemoveRange(0, bottomCardValue);
            deck.InsertRange(deck.Count - 1, countCutDeck);
        }

        // This step takes the numerical value of the card on the top of the deck.  We then count the cards in the deck starting with the top card on the deck.
        // The card BELOW the card we counted to is the Output Card; its numerical value will be returned by this step.
        // HOWEVER, if the output card happens to be a Joker, the program will disregard this value and start from Step 1 again.  This process will be handled within Program.cs.
        public int step5OutputCard() {
            int topCardValue = (int)deck[0];
			if (topCardValue == 0) {
				topCardValue = 53;
			}
            return (int)deck[topCardValue];
        }

        // This step checks the keystream value and determines if it's greater than 26 or not.  If it is greater than 26, it reduces the keystream value by 26.
        // If it's not greater than 26, then it leaves it alone and returns it as-is.
        public int step6ConvertToNumber(int keystreamValue) {
            if (keystreamValue > 26) {
                return keystreamValue - 26;
            }
            else {
                return keystreamValue;
            }
        }
    }
}