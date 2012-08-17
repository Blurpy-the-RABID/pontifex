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
		int[]plaintextNumbers; // This array will store the numerical values of the user's plaintext message.
		int[]keystreamNumbers; // This array will store the numerical values of the keystream numbers.
        int[]ciphertextNumbers; // This array will store the numerical values of the final ciphertext numbers.
        char[] ciphertextLetters; // This array will store the ciphertext message after it's been converted from numbers into letters.

        // The array below will be used in converting the ciphertext numbers into letters; once converted, the ciphertext letters will be stored in the ciphertextLetters[] array.
        char[] alphabet = new char[26] {'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z'};

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

        // This method will prompt the user for the message they wish to encrypt, and will calculate how many characters are within the message.
        public int getPlaintextMessage() {
            Console.Write("Enter the message that you wish to encrypt: ");
            origMessage = Console.ReadLine();
			origMessage = origMessage.ToUpper();
            origMessageLength = origMessage.Length;
			plaintextNumbers = new int[origMessageLength];
			keystreamNumbers = new int[origMessageLength];
            ciphertextNumbers = new int[origMessageLength];
            ciphertextLetters = new char[origMessageLength];
			for (int i = 0; i < plaintextNumbers.Length; i++) {
				plaintextNumbers[i] = (origMessage[i] - 'A') + 1;
			}
			return origMessageLength;
		}

        // This method will return the cipheretextLetters array for use in deciphering the first Deck's encrypted message.
        public char[] giveCiphertextLetters() {
            return ciphertextLetters;
        }

        // This method initializes the Deck instance with the ciphertextLetters[] array from the original Deck.
        public int getCiphertextLetters(char[] ciphertextLetterArray) {
            ciphertextLetters = ciphertextLetterArray;
            ciphertextNumbers = new int[ciphertextLetters.Length];
            plaintextNumbers = new int[ciphertextLetters.Length];
            keystreamNumbers = new int[ciphertextLetters.Length];
            for (int i = 0; i < ciphertextNumbers.Length; i++) {
                ciphertextNumbers[i] = (ciphertextLetterArray[i] - 'A') + 1;
            }
            return ciphertextLetters.Length;
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

        // This method will record each keystream value into the keystreamNumbers[] array.
		public void keystreamRecord(int counter, int keystreamValue) {
			keystreamNumbers[counter] = keystreamValue;
		}

        // This method will generate the final cipertext numbers into the ciphertextNumbers[] array.
        public void genCiphertextNumber() {
            for (int i = 0; i < ciphertextNumbers.Length; i++) {
                int cipherResult = plaintextNumbers[i] + keystreamNumbers[i];
                if (cipherResult > 26) {
                    cipherResult -= 26;
                }
                ciphertextNumbers[i] = cipherResult;
            }
        }

        // This method will convert the ciphertext numbers into letters, and then record each ciphertext letter into the ciphertextLetters[] array.
        public void ciphertextRecord() {
            for (int i = 0; i < ciphertextLetters.Length; i++) {
                char alphabetElement = alphabet[ciphertextNumbers[i] - 1];
                ciphertextLetters[i] = alphabetElement;
            }
        }

        // This method will allow us to see what the plaintext numbers are.
        public void plaintextDisplay() {
			Console.Write("Plaintext Numbers = ");
			for (int i = 0; i < plaintextNumbers.Length; i++) {
				Console.Write("{0} ", plaintextNumbers[i]);
			}
			Console.WriteLine();
		}

        // This method will allow us to see what the keystream numbers are.
		public void keystreamDisplay() {
			Console.Write("Keystream Numbers = ");
			for (int i = 0; i < keystreamNumbers.Length; i++) {
				Console.Write("{0} ", keystreamNumbers[i]);
			}
			Console.WriteLine();
		}

        // This method will allow us to see what the final ciphertext numbers are.
        public void ciphertextNumDisplay() {
            Console.Write("Ciphertext Numbers = ");
            for (int i = 0; i < ciphertextNumbers.Length; i++) {
                Console.Write("{0} ", ciphertextNumbers[i]);
            }
            Console.WriteLine();
        }

        // This method will allow us to see what the final ciphertext letters are.
        public void ciphertextLetterDisplay() {
            Console.Write("Ciphertext Letters = ");
            for (int i = 0; i < ciphertextLetters.Length; i++) {
                Console.Write("{0} ", ciphertextLetters[i]);
            }
            Console.WriteLine();
        }

        // To-Do List:
        // Next Big Step To Take:  Come up with a way to decipher the encrypted message by figuring out how to synchronize a second deck to the first deck.
    }
}