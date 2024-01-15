namespace Barcode
{
    public class BarcodeFixer
    {
        private const char BrokenSymbol = 'X';
        private const int LengthOfBarcode = 13;
        private const int EvenMultiplicationFactor = 3;
        private const int ModulusBase = 10;

        /// <summary>
        /// The Main method to process barcodes.
        /// </summary>
        static void Main(string[] args)
        {
            ProcessBarcode("40063X1333931");
            ProcessBarcode("40063813339X1");
            Console.ReadLine();
        }

        /// <summary>
        /// Processes a barcode by attempting to fix it and displaying the result.
        /// </summary>
        /// <param name="barcode">The barcode to process.</param>
        private static void ProcessBarcode(string barcode)
        {
            try
            {
                string fixedBarcode = FixBarcode(barcode);
                Console.WriteLine($"Fixed Barcode: {fixedBarcode}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// Fixes a barcode by replacing the broken symbol with the correct digit.
        /// </summary>
        /// <param name="barcode">The barcode to be fixed.</param>
        /// <returns>The fixed barcode if possible; otherwise, the original barcode.</returns>
        public static string FixBarcode(string barcode)
        {
            ValidateBarcode(barcode);

            int brokenIndex = barcode.IndexOf(BrokenSymbol);
            if (brokenIndex == -1) return barcode;

            int sum = CalculateSumExcludingBrokenDigit(barcode, brokenIndex);
            int missingDigit = CalculateMissingDigit(sum, IsEvenPosition(brokenIndex));

            return barcode.Replace(BrokenSymbol.ToString(), missingDigit.ToString());
        }

        /// <summary>
        /// Validates the barcode format and structure.
        /// </summary>
        /// <param name="barcode">The barcode to validate.</param>
        /// <exception cref="ArgumentException">Thrown when the barcode is invalid.</exception>
        private static void ValidateBarcode(string barcode)
        {
            if (barcode.Length != LengthOfBarcode)
                throw new ArgumentException("Invalid barcode length.");

            int brokenSymbolCount = 0;

            foreach (char c in barcode)
            {
                if (c == BrokenSymbol)
                {
                    brokenSymbolCount++;
                }
                else if (!char.IsDigit(c))
                {
                    throw new ArgumentException("Barcode contains non-numeric characters.");
                }
            }

            if (brokenSymbolCount > 1)
                throw new ArgumentException("Barcode must contain only one broken symbol.");
        }

        /// <summary>
        /// Calculates the sum of the digits in the barcode, excluding the broken digit.
        /// </summary>
        /// <param name="barcode">The barcode to calculate the sum for.</param>
        /// <param name="brokenIndex">The index of the broken digit.</param>
        /// <returns>The calculated sum.</returns>
        private static int CalculateSumExcludingBrokenDigit(string barcode, int brokenIndex)
        {
            int sum = 0;

            for (int i = 0; i < LengthOfBarcode; i++)
            {
                if (i == brokenIndex) continue;

                if (!char.IsDigit(barcode[i]))
                    throw new ArgumentException("Barcode contains non-numeric characters.");

                int digit = int.Parse(barcode[i].ToString());
                sum += IsEvenPosition(i) ? digit * EvenMultiplicationFactor : digit;
            }

            return sum;
        }

        /// <summary>
        /// Determines if the position of a digit in the barcode is an even position.
        /// </summary>
        /// <param name="index">The index of the digit in the barcode.</param>
        /// <returns>True if the position is even; otherwise, false.</returns>
        private static bool IsEvenPosition(int index)
        {
            // Note: Even positions are 1-based in barcodes.
            return (index % 2) == 1;
        }

        /// <summary>
        /// Calculates the missing digit in the barcode.
        /// </summary>
        /// <param name="sum">The sum of the digits in the barcode excluding the broken digit.</param>
        /// <param name="isEvenPosition">Indicates whether the broken digit is in an even position.</param>
        /// <returns>The missing digit.</returns>
        private static int CalculateMissingDigit(int sum, bool isEvenPosition)
        {
            int checksum = (ModulusBase - (sum % ModulusBase)) % ModulusBase;
            if (isEvenPosition)
            {
                for (int i = 0; i < ModulusBase; i++)
                {
                    if ((i * EvenMultiplicationFactor) % ModulusBase == checksum)
                        return i;
                }
            }

            return checksum;
        }
    }
}
