using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VincentFantini {

    class Program {

        static void Main(string[] args) {
            Deck cardDeck = new Deck();
            cardDeck.checkDeck();
            int messageLength = cardDeck.getMessage();

			for (int i = 0; i < messageLength; i++) {
				cardDeck.step1SJMove();
				cardDeck.step2LJMove();
				cardDeck.step3TripleCut();
				cardDeck.step4CountCut();
				int outputCardValue = cardDeck.step5OutputCard(); // This variable will contain the output card's value.

				while (outputCardValue == 0 || outputCardValue == 53) {
					cardDeck.step1SJMove();
					cardDeck.step2LJMove();
					cardDeck.step3TripleCut();
					cardDeck.step4CountCut();
					outputCardValue = cardDeck.step5OutputCard();
				}

				outputCardValue = cardDeck.step6ConvertToNumber(outputCardValue);
				// Console.WriteLine("FINAL Keystream Output Card Value = {0}", outputCardValue);
				cardDeck.keystreamRecord(i, outputCardValue);
				// cardDeck.checkDeck();
			}
            cardDeck.genCiphertextNumber();
            cardDeck.ciphertextRecord();

			cardDeck.plaintextDisplay();
			cardDeck.keystreamDisplay();
            cardDeck.ciphertextNumDisplay();
            cardDeck.ciphertextLetterDisplay();
        }
    }

}
