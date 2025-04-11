// See https://aka.ms/new-console-template for more information
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace ConsoleCalculator
{
    class Program
    {
        // Store calculation history
        static List<string> calculationHistory = new List<string>();

        static void Main(string[] args)
        {
            bool exitProgram = false;

            while (!exitProgram)
            {
                DisplayMainMenu();
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        PerformBasicCalculation();
                        break;
                    case "2":
                        PerformAdvancedCalculation();
                        break;
                    case "3":
                        ShowHistory();
                        break;
                    case "4":
                        ClearHistory();
                        break;
                    case "5":
                        exitProgram = true;
                        break;
                    default:
                        DisplayError("Invalid option. Please try again.");
                        PauseForUser();
                        break;
                }
            }

            DisplayColoredMessage("\nThank you for using the calculator! Press any key to exit.", ConsoleColor.Cyan);
            Console.ReadKey();
        }

        static void DisplayMainMenu()
        {
            Console.Clear();
            DisplayHeader();

            Console.WriteLine("│                   MAIN MENU                     │");
            Console.WriteLine("├────────────────────────────────────────────────┤");
            Console.WriteLine("│  1. Basic Calculation (2 numbers)               │");
            Console.WriteLine("│  2. Advanced Calculation (multiple numbers)     │");
            Console.WriteLine("│  3. View Calculation History                    │");
            Console.WriteLine("│  4. Clear History                               │");
            Console.WriteLine("│  5. Exit                                        │");
            Console.WriteLine("└────────────────────────────────────────────────┘");
            Console.Write("\nEnter your choice (1-5): ");
        }

        static void DisplayHeader()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("┌────────────────────────────────────────────────┐");
            Console.WriteLine("│             C# CONSOLE CALCULATOR              │");
            Console.WriteLine("├────────────────────────────────────────────────┤");
            Console.ResetColor();
        }

        static void PerformBasicCalculation()
        {
            Console.Clear();
            DisplayHeader();

            try
            {
                Console.WriteLine("│               BASIC CALCULATION                │");
                Console.WriteLine("└────────────────────────────────────────────────┘");

                // Get the first number
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("\nEnter the first number: ");
                Console.ResetColor();
                double num1 = Convert.ToDouble(Console.ReadLine());

                // Get the operation
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("Enter an operation (+, -, *, /, %): ");
                Console.ResetColor();
                string operation = Console.ReadLine();

                // Get the second number
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("Enter the second number: ");
                Console.ResetColor();
                double num2 = Convert.ToDouble(Console.ReadLine());

                // Perform the calculation
                double result = 0;

                switch (operation)
                {
                    case "+":
                        result = num1 + num2;
                        break;
                    case "-":
                        result = num1 - num2;
                        break;
                    case "*":
                        result = num1 * num2;
                        break;
                    case "/":
                        if (num2 == 0)
                        {
                            throw new DivideByZeroException("Cannot divide by zero!");
                        }
                        result = num1 / num2;
                        break;
                    case "%":
                        if (num2 == 0)
                        {
                            throw new DivideByZeroException("Cannot perform modulo by zero!");
                        }
                        result = num1 % num2;
                        break;
                    default:
                        throw new InvalidOperationException("Operation not recognized.");
                }

                // Create calculation text and add to history
                string calculation = $"{num1} {operation} {num2} = {result}";
                calculationHistory.Add(calculation);

                // Display result with visual box
                DisplayResult(calculation);
            }
            catch (FormatException)
            {
                DisplayError("Please enter valid numbers.");
            }
            catch (DivideByZeroException ex)
            {
                DisplayError(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                DisplayError(ex.Message);
            }
            catch (Exception ex)
            {
                DisplayError($"An unexpected error occurred: {ex.Message}");
            }

            PauseForUser();
        }

        static void PerformAdvancedCalculation()
        {
            Console.Clear();
            DisplayHeader();

            try
            {
                Console.WriteLine("│            ADVANCED CALCULATION                │");
                Console.WriteLine("└────────────────────────────────────────────────┘");
                Console.WriteLine("\nEnter a mathematical expression with multiple numbers.");
                Console.WriteLine("Examples:");
                Console.WriteLine("  • 5 + 10 - 3 * 2");
                Console.WriteLine("  • (10 + 5) * 2 / 3");
                Console.WriteLine("  • 7 % 3 + 4 * 2");

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("\nEnter your expression: ");
                Console.ResetColor();
                string expression = Console.ReadLine();

                // Use DataTable to evaluate the expression
                DataTable dt = new DataTable();
                var result = dt.Compute(expression, "");

                // Create calculation text and add to history
                string calculation = $"{expression} = {result}";
                calculationHistory.Add(calculation);

                // Display result with visual box
                DisplayResult(calculation);
            }
            catch (SyntaxErrorException)
            {
                DisplayError("Invalid expression syntax. Please check your expression.");
            }
            catch (EvaluateException)
            {
                DisplayError("Error evaluating the expression. Please check your numbers and operators.");
            }
            catch (DivideByZeroException)
            {
                DisplayError("Cannot divide by zero!");
            }
            catch (Exception ex)
            {
                DisplayError($"An unexpected error occurred: {ex.Message}");
            }

            PauseForUser();
        }

        static void DisplayResult(string calculation)
        {
            Console.WriteLine("\n┌────────────────────────────────────────────────┐");
            Console.WriteLine("│                   RESULT                       │");
            Console.WriteLine("├────────────────────────────────────────────────┤");
            Console.ForegroundColor = ConsoleColor.Green;

            // Handle longer calculations with wrapping
            if (calculation.Length <= 46)
            {
                Console.WriteLine($"│  {calculation.PadRight(46)}│");
            }
            else
            {
                // Split long results into multiple lines if needed
                for (int i = 0; i < calculation.Length; i += 46)
                {
                    string line = i + 46 <= calculation.Length ?
                        calculation.Substring(i, 46) :
                        calculation.Substring(i);
                    Console.WriteLine($"│  {line.PadRight(46)}│");
                }
            }

            Console.ResetColor();
            Console.WriteLine("└────────────────────────────────────────────────┘");
        }

        static void ShowHistory()
        {
            Console.Clear();
            DisplayHeader();

            Console.WriteLine("│               CALCULATION HISTORY               │");
            Console.WriteLine("└────────────────────────────────────────────────┘\n");

            if (calculationHistory.Count == 0)
            {
                Console.WriteLine("No calculations have been performed yet.");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("   #  |  Calculation");
                Console.WriteLine("------+------------------------------------------");
                Console.ResetColor();

                for (int i = 0; i < calculationHistory.Count; i++)
                {
                    string calcText = calculationHistory[i];
                    if (calcText.Length > 50)
                    {
                        calcText = calcText.Substring(0, 47) + "...";
                    }
                    Console.WriteLine($"  {i + 1,2}  |  {calcText}");
                }
            }

            PauseForUser();
        }

        static void ClearHistory()
        {
            Console.Clear();
            DisplayHeader();

            Console.WriteLine("│               CLEAR HISTORY                     │");
            Console.WriteLine("└────────────────────────────────────────────────┘\n");

            Console.Write("Are you sure you want to clear the calculation history? (Y/N): ");
            string answer = Console.ReadLine().ToUpper();

            if (answer == "Y")
            {
                calculationHistory.Clear();
                DisplayColoredMessage("\nHistory has been cleared successfully!", ConsoleColor.Green);
            }
            else
            {
                Console.WriteLine("\nOperation cancelled. History remains intact.");
            }

            PauseForUser();
        }

        static void DisplayError(string message)
        {
            Console.WriteLine("\n┌────────────────────────────────────────────────┐");
            Console.WriteLine("│                   ERROR                         │");
            Console.WriteLine("├────────────────────────────────────────────────┤");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"│  {message.PadRight(46)}│");
            Console.ResetColor();
            Console.WriteLine("└────────────────────────────────────────────────┘");
        }

        static void DisplayColoredMessage(string message, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        static void PauseForUser()
        {
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
    }
}
