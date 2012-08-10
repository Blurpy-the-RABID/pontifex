using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VincentFantini {

    class Deck {

        string origMessage; // This variable will contain the user's plaintext message that is to be encrypted.
        int origMessageLength; // This variable will contain the number of characters in the user's plaintext message.
        int sjLocation = 0; // This variable will contain the location of the Small Joker card.
        int ljLocation = 0; // This variable will contain the location of the Large Joker card.
		int[]messageNumbers; // This array will store the numerical values of the user's plaintext message.
		int[]keystreamNumbers; // This array will store the numerical values of the keystream numbers.

        // Next, we create the playing cards and assign an integer value to each card based on their value in the Pontifex encryption algorithm.
        // The cards are sorted in their numerical values, and the suits are arranged in bridge order (Clubs, then Diamonds, then Hearts, then Spades).
        // At the end of the deck are the Small Joker (SJ) and the Large Joker (LJ).  Both Jokers have the same numerical value (53).

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
                // Insert deck shuffling commands here.
            }
        }

        // This method will prompt the user for the message they wish to encrypt, and will calculate how many characters are within the message.
        public int getMessage() {
            Console.Write("Enter the message that you wish to encrypt: ");
            origMessage = Console.ReadLine();
			origMessage = origMessage.ToUpper();
            origMessageLength = origMessage.Length;
			messageNumbers = new int[origMessageLength];
			keystreamNumbers = new int[origMessageLength];
			for (int i = 0; i < messageNumbers.Length; i++) {
				messageNumbers[i] = (origMessage[i] - 'A') + 1;
			}
			return origMessageLength;
		}

        public void step1SJMove() {
            // First, we locate the Small Joker in the deck array.
            Console.WriteLine("Step 1:  Find & Move The Small Joker");
            for (int i = 0; i < deck.Count; i++) {
                if ((Cards)deck[i] == Cards.SJ) {
                    sjLocation = i;
                    Console.WriteLine("Small Joker Location is {0}", sjLocation);
                }
            }

            // Now that we've found the Small Joker, we will now shift its position one space down within the deck.
            // If its starting position is on the bottom of the deck, it will be shifted to the position immediately after the first card in the deck.  We'll check for this first...
            if (sjLocation == deck.Count - 1) {
                deck.RemoveAt(sjLocation);
                deck.Insert(1, Cards.SJ);
            }
            else {
                deck.RemoveAt(sjLocation);
                deck.Insert(sjLocation + 1, Cards.SJ);
            }

            for (int i = 0; i < deck.Count; i++) {
                Console.WriteLine("Card {0} is {1}; Value = {2}", i, deck[i], (int)deck[i]);
            }
        }

        public void step2LJMove() {
            // Next, we locate the Large Joker in the deck array.
            Console.WriteLine("Step 2:  Find & Move The Large Joker");
            for (int i = 0; i < deck.Count; i++) {
                if ((Cards)deck[i] == Cards.LJ) {
                    ljLocation = i;
                    Console.WriteLine("Large Joker Location is {0}", ljLocation);
                }
            }

            // Now that we've found the Large Joker, we will now shift its position two spaces down within the deck.
            // If its starting position is on the bottom of the deck, it will be shifted to the position immediately after the second card in the deck.
            // If the Large Joker is located one card above the bottom card in the deck, it's to be shifted to just below the top card.
            if (ljLocation == deck.Count - 1) {
				deck.RemoveAt(ljLocation);
                deck.Insert(2, Cards.LJ);
            }
            else if (ljLocation == deck.Count - 2) {
				deck.RemoveAt(ljLocation);
                deck.Insert(1, Cards.LJ);
            }
            else {
				deck.RemoveAt(ljLocation);
                deck.Insert(ljLocation + 2, Cards.LJ);
            }

            for (int i = 0; i < deck.Count; i++) {
                Console.WriteLine("Card {0} is {1}; Value = {2}", i, deck[i], (int)deck[i]);
            }
        }

        public void step3TripleCut() {
            // Step 3 is where we perform the triple cut - we swap out all of the cards above the Small Joker with the cards below the Large Joker.
            // The order of each of these parts of the deck are to remain in the order that they're in.
            // To do this, we'll create three separate ArrayLists that will contain each cut of the deck.  We'll then empty out the deck ArrayList,
            // and then concatenate the three separate ArrayLists back into the deck ArrayList in their proper order.

            // First, we locate the Small & Large Jokers in the deck array.
            Console.WriteLine("Step 3:  Triple-Cut The Deck");
            for (int i = 0; i < deck.Count; i++) {
                if ((Cards)deck[i] == Cards.SJ) {
                    sjLocation = i;
                    Console.WriteLine("Small Joker Location is {0}", sjLocation);
                }
            }

            for (int i = 0; i < deck.Count; i++) {
                if ((Cards)deck[i] == Cards.LJ) {
                    ljLocation = i;
                    Console.WriteLine("Large Joker Location is {0}", ljLocation);
                }
            }

            // Now we create a new ArrayList to copy the portions of the deck above & below the Jokers.
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
                        Console.WriteLine("The Small Joker is on the top of the deck."); 
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
                        Console.WriteLine("The Large Joker is on the bottom of the deck.");
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
                        Console.WriteLine("The Large Joker is on the top of the deck.");
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
                        Console.WriteLine("The Small Joker is on the bottom of the deck.");
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
                
                // Since the deck ArrayList has now been changed in size, we'll relocate the Jokers in order to properly determine which cards are below the Second Joker.
                for (int i = 0; i < deck.Count; i++) {
                    if ((Cards)deck[i] == Cards.SJ) {
                        sjLocation = i;
                        Console.WriteLine("Small Joker Location is {0}", sjLocation);
                    }
                    if ((Cards)deck[i] == Cards.LJ) {
                        ljLocation = i;
                        Console.WriteLine("Large Joker Location is {0}", ljLocation);
                    }
                }

                // We've found the Jokers, so we quickly determine which is the Second Joker and then remove the cards below it.
                if (sjLocation > ljLocation) {
                    deck.RemoveRange(sjLocation + 1, deckBelowSJ.Count);
                }
                else if (sjLocation < ljLocation) {
                    deck.RemoveRange(ljLocation + 1, deckBelowSJ.Count);
                }

                // The following code displays what's contained within each ArrayList to ensure that the deck has been properly copied above & below the Jokers.
                // This code can be deleted later when testing & troubleshooting have been completed, and the interface is to be cleaned up.
                Console.WriteLine("Cards contained within deckAboveFJ {0}:", deckAboveFJ.Count);
                for (int i = 0; i < deckAboveFJ.Count; i++) {
                    Console.WriteLine("Card {0} is {1}; Value = {2}", i, deckAboveFJ[i], (int)deckAboveFJ[i]);
                }
                Console.WriteLine("Cards contained within deckBelowSJ {0}:", deckBelowSJ.Count);
                for (int i = 0; i < deckBelowSJ.Count; i++) {
                    Console.WriteLine("Card {0} is {1}; Value = {2}", i, deckBelowSJ[i], (int)deckBelowSJ[i]);
                }

                Console.WriteLine("Cards contained within the Deck (between the Jokers){0}:", deck.Count);
                for (int i = 0; i < deck.Count; i++) {
                    Console.WriteLine("Card {0} is {1}; Value = {2}", i, deck[i], (int)deck[i]);
                }

                // Finally, we re-order the deck ArrayList so that the top & bottom cut portions of the deck are swapped.
                deck.InsertRange(0, deckBelowSJ);
                deck.InsertRange(deck.Count, deckAboveFJ);
                Console.WriteLine("Cards contained within the Triple-Cut Deck (Cards Total = {0}):", deck.Count);
                for (int i = 0; i < deck.Count; i++) {
                    Console.WriteLine("Card {0} is {1}; Value = {2}", i, deck[i], (int)deck[i]);
                }

				deckAboveFJ.Clear();
				deckBelowSJ.Clear();
            }
        }

        public void step4CountCut() {
            // For this step, we check the bottom card in the deck and check its numerical value.  This value is the number of cards we'll be counting from the top of the deck.
            // We then cut the counted cards from the bottom of the deck, and we place them just above the last card in the deck.
            Console.WriteLine("Step 4:  Count-Cut The Deck");
            int bottomCardValue = (int)deck[deck.Count - 1]; // The bottom card's value is stored in this variable.
            Console.WriteLine("Bottom Card = {0}; Card Value = {1}", deck[deck.Count - 1], (int)deck[deck.Count - 1]);
            Console.WriteLine("Cards to be count-cut from the top of the deck = {0}", bottomCardValue);

            // Now to count & copy the cards from the top of the deck and store them in an ArrayList.
            ArrayList countCutDeck = new ArrayList(); // This will copy the cards counted from the top of the deck.

            for (int i = 0; i < bottomCardValue && i < deck.Count; i++) {
                Console.WriteLine("Card {0} is count-cut from the top of the deck.", deck[i]);
                countCutDeck.Add(deck[i]);
            }

            Console.WriteLine("Number of cards in countCutDeck ArrayList = {0}.", countCutDeck.Count);
            deck.RemoveRange(0, bottomCardValue);
            deck.InsertRange(deck.Count - 1, countCutDeck);
            for (int i = 0; i < deck.Count; i++) {
                Console.WriteLine("Card {0} is {1}; Value = {2}", i, deck[i], (int)deck[i]);
            }
        }

        public int step5OutputCard() {
            // This step takes the numerical value of the card on the top of the deck.  We then count the cards in the deck starting with the top card on the deck.
            // The card BELOW the card we counted to is the Output Card; its numerical value will be returned by this step.
            // HOWEVER, if the output card happens to be a Joker, the program will disregard this value and start from Step 1 again.  This process will be handled within Program.cs.
            Console.WriteLine("Step 5:  Find The Output Card");
            int topCardValue = (int)deck[0];
			if (topCardValue == 0) {
				topCardValue = 53;
			}
            Console.WriteLine("Value of Top Card {0} is {1}", deck[0], (int)deck[0]);
            Console.WriteLine("Value of Output Card {0} is {1}", deck[topCardValue], (int)deck[topCardValue]);
            return (int)deck[topCardValue];
        }

        public int step6ConvertToNumber(int keystreamValue) {
            // This step checks the keystream value and determines if it's greater than 26 or not.  If it is greater than 26, it reduces the keystream value by 26.
            // If it's not greater than 26, then it leaves it alone and returns it as-is.
            Console.WriteLine("Step 6:  Convert The Output To A Number");
            if (keystreamValue > 26) {
                return keystreamValue - 26;
            }
            else {
                return keystreamValue;
            }
        }

		public void keystreamRecord(int counter, int keystreamValue) {
			// This method will record each keystream value into the keystreamNumbers[] array.
			keystreamNumbers[counter] = keystreamValue;
		}

		public void plaintextDisplay() {
			// This method will allow us to see what the plaintext numbers are.
			Console.Write("Plaintext Numbers = ");
			for (int i = 0; i < messageNumbers.Length; i++) {
				Console.Write("{0} ", messageNumbers[i]);
			}
			Console.WriteLine();
		}

		public void keystreamDisplay() {
			// This method will allow us to see what the keystream numbers are.
			Console.Write("Keystream Numbers = ");
			for (int i = 0; i < keystreamNumbers.Length; i++) {
				Console.Write("{0} ", keystreamNumbers[i]);
			}
			Console.WriteLine();
		}

        // To-Do List:
        // - Get the commands in Program.cs to execute for each of the plaintext characters that the user types in during the getMessage() step.
        // - Store the output numbers somewhere in the order that they're created (use an array or ArrayList).
        // - Create a method to convert the plaintext characters from the getMessage() method into numerical values (a = 1...z = 26), and then store
		//		those numbers somewhere.
        // - Create a method to properly add the plaintext numbers to the keystream numbers (modulo 26) to create the final ciphertext numbers.
        // - Create a method to properly convert the ciphertext numbers into letters (which is the final encrypted message).
        
        // Next Big Step To Take:  Come up with a way to decipher the encrypted message by figuring out how to synchronize a second deck to the first deck.
    }
}