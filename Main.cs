using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;

namespace HSTUPL__hopefully_simple_to_understand_programming_language_
{   
    class Init
    {   
        public static string[] keywords = 
        {
            "declare",
            "assign",
            "to",
            "print",
            "repeat"
        };
        public static List<string> variables = new List<string>();
        public static List<string> variableValues = new List<string>();
        public static string opening = "'";
        public static string ending = "'";
        public static int i;
    }

    class Interpreter
    {   
        public static string tempString;
        public static string[] segments;
        public static void Interpret(string input)
        {   
            tempString = input.Split(' ')[1]; // checking first word (for repeat)

            if(tempString == "repeat")
            {
                Repeat(input);
            }

            tempString = input.Split(' ')[1]; // retrieves second word (to check operands)

            string[] operations = 
            {
                "+",
                "-",
                "*",
                "/",
                "^",
                "!=",
                "==",
                "<=",
                ">=",
                "<",
                ">"
            };
            if (operations.Contains(tempString))
            {
                Calculate(input);
            }
            string[] variableOperations =
            {
                "++",
                "--",
            };
            if (variableOperations.Contains(tempString))
            {
                ApplyToVariable(input);
            }

            tempString = input.Split(' ')[0]; // retrieves first word

            switch(tempString)
            {
                case "declare":
                {   
                    tempString = input.Split(' ').Last();
                    Init.variables.Add(tempString);
                    Init.variableValues.Add(" ");
                    Run.Main();
                    break;
                }
                case "assign":
                {
                    string[] substrings = input.Split(' ');
                    tempString = Convert.ToString(substrings[2]); // I THOUGHT I FIXED THE INDEXING BUG WHY WAS THIS 3??? oh well i fixed it anyways
                    if(Init.variables.Contains(tempString)) 
                    {   
                        Init.i = Init.variables.IndexOf(Convert.ToString(substrings[0]));
                        Init.variableValues.RemoveAt(Init.i);
                        Init.variableValues.Insert(Init.i, substrings[2]);
                    }
                    else
                    {
                        Errors.VariableNotDeclaredError(substrings[0]);
                    }
                    Run.Main();
                    break;
                }
                case "print":
                {
                    if(input.Contains("'")) // string
                    {   
                        tempString = Misc.GetStringBetweenCharacters(input, Convert.ToChar(Init.opening), Convert.ToChar(Init.ending));
                        Console.WriteLine(tempString);
                    }
                    else // variable 
                    {
                        tempString = input.Split(' ').Last();
                        if(Init.variables.Contains(tempString))
                        {
                            Init.i = Init.variables.IndexOf(tempString);
                            Console.WriteLine(Init.variableValues[Init.i]);
                        }
                        else
                        {
                            Errors.VariableNotDeclaredError(tempString);
                        }
                    }
                    Run.Main();
                    break;
                }
                default:
                {
                    Errors.UnknownKeywordError(tempString);
                    break;
                }
            }
        }

        public static void Calculate(string input)
        {
            string[] segments = input.Split(' ');
            
            if(Init.variables.Contains(segments[0]))
            {
                Init.i = Init.variables.IndexOf(segments[0]);
                Interpreter.tempString = Init.variableValues[Init.i];
                segments[0] = Interpreter.tempString;
            }
            
            if(Init.variables.Contains(segments[2]))
            {
                Init.i = Init.variables.IndexOf(segments[2]);
                Interpreter.tempString = Init.variableValues[Init.i];
                segments[2] = Interpreter.tempString;
            }

            switch(segments[1])
            {
                case "+":
                {
                    Console.WriteLine(Convert.ToInt32(segments[0]) + Convert.ToInt32(segments[2]));
                    break;
                }
                case "-":
                {
                    Console.WriteLine(Convert.ToInt32(segments[0]) - Convert.ToInt32(segments[2]));
                    break;
                }
                case "*":
                {
                    Console.WriteLine(Convert.ToInt32(segments[0]) * Convert.ToInt32(segments[2]));
                    break;
                }
                case "/":
                {
                    try
                    {
                        Console.WriteLine(Convert.ToInt32(segments[0]) / Convert.ToInt32(segments[2]));
                    }
                    catch (DivideByZeroException)
                    {
                        Errors.DivisionByZeroError(input);
                    }
                    break;
                }
                case "^":
                {
                    Console.WriteLine(Math.Pow(Convert.ToInt32(segments[0]), Convert.ToInt32(segments[2])));
                    break;
                }
                case "!=":
                {
                    Console.WriteLine(Convert.ToInt32(segments[0]) != Convert.ToInt32(segments[2]));
                    break;
                }
                case "==":
                {
                    Console.WriteLine(Convert.ToInt32(segments[0]) == Convert.ToInt32(segments[2]));
                    break;
                }
                case "<=":
                {
                    Console.WriteLine(Convert.ToInt32(segments[0]) <= Convert.ToInt32(segments[2]));
                    break;
                }
                case ">=":
                {
                    Console.WriteLine(Convert.ToInt32(segments[0]) >= Convert.ToInt32(segments[2]));
                    break;
                }
                case "<":
                {
                    Console.WriteLine(Convert.ToInt32(segments[0]) < Convert.ToInt32(segments[2]));
                    break;
                }
                case ">":
                {
                    Console.WriteLine(Convert.ToInt32(segments[0]) > Convert.ToInt32(segments[2]));
                    break;
                }
                    
            }
            Run.Main();
        }

        public static void ApplyToVariable(string input)
        {
            if(input.Contains("++"))
            {
                segments = input.Split('+');
            }
            else
            {
                segments = input.Split('-');
            }
            Console.WriteLine(segments[0]);
            Console.WriteLine(segments[1]);
        }

        public static void Repeat(string input)
        {
            segments = input.Split(' ');
            Interpreter.tempString = segments[2];
            tempString.Trim(':');
            Init.i = Convert.ToInt32(tempString);
            input = "";
            for(int j = 2; j == segments.Length; j++)
            {
                input += segments[j];
                input += " ";
            }
            Console.WriteLine(input);
            Console.ReadKey();
            /*for(int k = 0; k == Init.i; k++)
            {
                Interpreter.Interpret(input);
            }*/
        }
    }

    class Errors
    {
        public static void UnknownKeywordError(string keyword)
        {
            Console.WriteLine("The keyword " + keyword + " is not valid. Please try again.");
            Run.Main();
        }
        
        public static void VariableNotDeclaredError(string varName)
        {
            Console.WriteLine("The variable named " + varName + " does not exist. Have you declared it?");
            Run.Main();
        }
        public static void MissingDirectoryError(string filename)
        {
            Console.WriteLine("The file at directory " + filename + " does not exist. Check if the directory is correct or if the file has been deleted.");
            Run.Main();
        }
        public static void DivisionByZeroError(string input)
        {
            Console.WriteLine("Cannot divide by 0. Please try again.");
            Run.Main();
        }
    }
    class Run
    {
        public static void Main()
        {   
            Console.Title = "HSTUPL C# Interpreter";

            Console.Write("HSTUPL > ");
            string input = Console.ReadLine();
            Interpreter.Interpret(input);

            Console.ReadKey(); // instaclose prevention
        }    
    }

    class Misc
    {
        public static string GetStringBetweenCharacters(string input, char charFrom, char charTo)
        {
        int posFrom = input.IndexOf(charFrom);
        if (posFrom != -1) // if found char
        {
            int posTo = input.IndexOf(charTo, posFrom + 1);
            if (posTo != -1) // if found char
            {
                return input.Substring(posFrom + 1, posTo - posFrom - 1);
            }
        }

        return string.Empty;
        }
    }
}
