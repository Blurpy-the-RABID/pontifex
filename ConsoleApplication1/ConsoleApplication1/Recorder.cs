using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VincentFantini {

    // The purpose of the Recorder class is to handle the recording, encrypting, and deciphering of
    // messages.  From an object-oriented perspective, this class is the pad of paper that you would use
    // to help you write down and mathematically encrypt/decipher a message.  The deck is a separate
    // object whose only purpose is to generate keystream numbers.

    class Recorder {
        string plaintextMessage; // This variable will contain the user's plaintext message that is to be encrypted.
        int plaintextMessageLength; // This variable will contain the number of characters in the user's plaintext message.
        int[] plaintextNumbers; // This array will store the numerical values of the user's plaintext message.
        int[] keystreamNumbers; // This array will store the numerical values of the keystream numbers.
        int[] ciphertextNumbers; // This array will store the numerical values of the final ciphertext message's characters.
        char[] ciphertextLetters; // This array will store the characters of the ciphertext message after it's been converted from numbers into letters.

        int[] decipheredMessageNumbers; // This array will store the numerical values of the final deciphered message's characters.
        char[] decipheredMessageLetters; // This array will store the characters of the final deciphered message after it's converted from numbers into letters.

        // The array below will be used in converting the ciphertext numbers into letters; once converted, the ciphertext letters will be stored in the ciphertextLetters[] array.
        char[] alphabet = new char[26] { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };

        // This method will prompt the user for the message they wish to encrypt, and will calculate how many characters are within the message.
        // This method will then initialize all of the arrays above so that they match the user's message length.
        public int getPlaintextMessage() {
            Console.Write("Enter the message that you wish to encrypt: ");
            plaintextMessage = Console.ReadLine();
            plaintextMessage = plaintextMessage.ToUpper();
            plaintextMessageLength = plaintextMessage.Length;
            plaintextNumbers = new int[plaintextMessageLength];
            keystreamNumbers = new int[plaintextMessageLength];
            ciphertextNumbers = new int[plaintextMessageLength];
            ciphertextLetters = new char[plaintextMessageLength];
            for (int i = 0; i < plaintextNumbers.Length; i++) {
                plaintextNumbers[i] = (plaintextMessage[i] - 'A') + 1;
            }
            return plaintextMessageLength;
        }

        // This method will return the cipheretextLetters array for use in deciphering the first Deck's encrypted message.
        public char[] giveCiphertextLetters() {
            return ciphertextLetters;
        }

        // This method is similar to the getPlaintextMessage() method in that it's to initialize the decipheredMessageNumbers[] & decipheredMessageLetters[] arrays.
        // It takes the generated ciphertext message and uses it much like how the getPlaintextMessage() method uses the plaintextMessage string variable to initialize the other arrays.
        public int getCiphertextMessage() {
            decipheredMessageNumbers = new int[ciphertextLetters.Length];
            decipheredMessageLetters = new char[ciphertextLetters.Length];

            // The following for statement will set all of the keystreamNumbers[] array elements to zero so that we can ensure that the program doesn't "cheat" by recycling the
            // keystream numbers that were already generated when the plaintext messasge was originally encrypted.
            for (int i = 0; i < keystreamNumbers.Length; i++) {
                keystreamNumbers[i] = 0;
            }
            return ciphertextLetters.Length;
        }

        // This method will record each keystream value into the keystreamNumbers[] array.
        public void keystreamRecord(int counter, int keystreamValue) {
            keystreamNumbers[counter] = keystreamValue;
        }

        // This method will generate the final ciphertext numbers into the ciphertextNumbers[] array.
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

        // This method will generate the final deciphered plaintext numbers into the decipheredMessageNumbers[] array.
        public void genDecipheredMessageNumber() {
            // The formula for deciphering an encrypted message is to first convert each ciphertext character into a number.  We then generate a keystream number for each character
            // of the ciphertext message, and then subtract the keystream number from its corresponding ciphertext number, modulo 26.  So 22-1 = 21, 1-22 = 5.
            // If the ciphertext number is less than or equal to its corresponding keystream number, then we add 26 to the ciphertext number to ensure that we don't get a negative
            // number.  So in the case of 1-22 = 5, we add 26 to 1 and we get 27-22 = 5.
            for (int i = 0; i < decipheredMessageNumbers.Length; i++) {
                if (ciphertextNumbers[i] <= keystreamNumbers[i]) {
                    ciphertextNumbers[i] += 26;
                }
                int decipherResult = ciphertextNumbers[i] - keystreamNumbers[i];
                decipheredMessageNumbers[i] = decipherResult;
            }
        }

        // This method will convert the ciphertext numbers into letters, and then record each ciphertext letter into the ciphertextLetters[] array.
        public void decipheredMessageRecord() {
            for (int i = 0; i < decipheredMessageLetters.Length; i++) {
                char alphabetElement = alphabet[decipheredMessageNumbers[i] - 1];
                decipheredMessageLetters[i] = alphabetElement;
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

        // This method will allow us to see what the final deciphered message numbers are.
        public void decipheredMessageNumberDisplay() {
            Console.Write("Deciphered Message Numbers = ");
            for (int i = 0; i < decipheredMessageNumbers.Length; i++) {
                Console.Write("{0} ", decipheredMessageNumbers[i]);
            }
            Console.WriteLine();
        }

        // This method will allow us to see what the final deciphered message letters are.
        public void decipheredMessageLetterDisplay() {
            Console.Write("Deciphered Message Letters = ");
            for (int i = 0; i < decipheredMessageLetters.Length; i++) {
                Console.Write("{0} ", decipheredMessageLetters[i]);
            }
            Console.WriteLine();
        }
    }
}
