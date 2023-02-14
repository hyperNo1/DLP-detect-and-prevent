using System;
using System.IO;
using System.Text.RegularExpressions;

namespace DataLeakagePrevention
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter the file path to scan for data leakage: ");
            string filePath = Console.ReadLine();

            // Read the file into a string
            string fileContent = File.ReadAllText(filePath);

            // Define regex patterns for sensitive data
            string creditCardRegex = @"\b(?:\d[ -]*?){13,16}\b";
            string emailRegex = @"\b[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Z|a-z]{2,}\b";
            string socialSecurityNumberRegex = @"\b\d{3}[- ]?\d{2}[- ]?\d{4}\b";

            // Use regex to search for sensitive data in the file
            MatchCollection creditCardMatches = Regex.Matches(fileContent, creditCardRegex);
            MatchCollection emailMatches = Regex.Matches(fileContent, emailRegex);
            MatchCollection socialSecurityNumberMatches = Regex.Matches(fileContent, socialSecurityNumberRegex);

            // Check if any sensitive data was found
            if (creditCardMatches.Count > 0 || emailMatches.Count > 0 || socialSecurityNumberMatches.Count > 0)
            {
                Console.WriteLine("Data leakage detected in the file!");
                Console.WriteLine("Credit card numbers found: " + creditCardMatches.Count);
                Console.WriteLine("Email addresses found: " + emailMatches.Count);
                Console.WriteLine("Social security numbers found: " + socialSecurityNumberMatches.Count);

                // Option to redact the sensitive data
                Console.WriteLine("Do you want to redact the sensitive data? (y/n)");
                string answer = Console.ReadLine();

                if (answer == "y")
                {
                    // Replace the sensitive data with asterisks
                    fileContent = Regex.Replace(fileContent, creditCardRegex, "*****");
                    fileContent = Regex.Replace(fileContent, emailRegex, "*****");
                    fileContent = Regex.Replace(fileContent, socialSecurityNumberRegex, "*****");

                    // Save the redacted file
                    File.WriteAllText(filePath, fileContent);
                    Console.WriteLine("Sensitive data has been redacted.");
                }
            }
            else
            {
                Console.WriteLine("No data leakage detected.");
            }
        }
    }
}
