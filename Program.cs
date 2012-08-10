using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VincentFantini {

    class Program {

        static void Main(string[] args) {
            Deck cardDeck = new Deck();
            cardDeck.checkDeck();
            cardDeck.getMessage();
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
            Console.WriteLine("FINAL Output Card Value = {0}", outputCardValue);
        }

    }

}
