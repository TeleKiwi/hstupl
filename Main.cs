using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

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
            "repeat",
            "run"
        };
        public static List<string> variables = new List<string>();
        public static List<string> variableValues = new List<string>();
        public static string opening = "'";
        public static string ending = "'";
        public static int i;
        public static int placeholderInt;
    }

    class Interpreter
    {   
        public static string tempString;
        public static string[] segments;
        public static void Interpret(string input)
        {   
            tempString = input.Split(' ')[0]; // checking first word (for repeat/++ or --)
    
            if(tempString.Contains("repeat"))
            {
                Repeat(input);
            }

            if(tempString.Contains("++") || tempString.Contains("--"))
            {
                ApplyToVariable(input);
            }

            try
            {
                tempString = input.Split(' ')[1]; // retrieves second word (to check operands)
            }
            catch(IndexOutOfRangeException)
            {
                Errors.UnknownKeywordError(input.Split(' ')[0]); 
            }
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

            tempString = input.Split(' ')[0]; // retrieves first word

            switch(tempString)
            {
                case "declare":
                {   
                    if(Init.keywords.Contains(input.Split(' ')[1]))
                    {
                        Errors.VarCannotShareNameWithKeyword(input.Split(' ')[1]);
                    }
                    else
                    {
                        tempString = input.Split(' ').Last();
                        Init.variables.Add(tempString);
                        Init.variableValues.Add(" ");
                        Run.Main();
                    }
                    break;
                }
                case "assign":
                {
                    string[] substrings = input.Split(' ');
                    tempString = Convert.ToString(substrings[3]); 
                    if(Init.variables.Contains(tempString)) 
                    {   
                        Init.i = Init.variables.IndexOf(Convert.ToString(substrings[3]));
                        Init.variableValues.RemoveAt(Init.i);
                        Init.variableValues.Insert(Init.i, substrings[1]);
                    }
                    else
                    {
                        Errors.VariableNotDeclaredError(substrings[3]);
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
                case "run":
                {
                    string[] inputSegmented = input.Split(' ');
                    ExecuteFile(inputSegmented[1]);
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
                // convert to int, increase int by 1, then store the int into array
                segments = input.Split('+');
                Init.i = Init.variables.IndexOf(segments[0]);
                tempString = Init.variableValues[Init.i];
                try
                {
                    Init.placeholderInt = Convert.ToInt32(tempString); 
                }
                catch(FormatException)
                {
                    Errors.UnassignedVarError(segments[0]);
                }
                Init.placeholderInt++;
                tempString = Convert.ToString(Init.placeholderInt);
                Init.variableValues[Init.i] = tempString;
            }
            else
            {
                segments = input.Split('-');
                Init.i = Init.variables.IndexOf(segments[0]);
                tempString = Init.variableValues[Init.i];
                try
                {
                    Init.placeholderInt = Convert.ToInt32(tempString); 
                }
                catch(FormatException)
                {
                    Errors.UnassignedVarError(segments[0]);
                }
                Init.placeholderInt--;
                tempString = Convert.ToString(Init.placeholderInt);
                Init.variableValues[Init.i] = tempString;
            }
            Run.Main();
        }

        public static void Repeat(string input)
        {
            segments = input.Split(' ');
            Interpreter.tempString = segments[1];
            /*tempString.Trim(new char[] {':'} );
            Console.WriteLine(tempString);
            Console.WriteLine(segments[1]);
            string pattern = "[:]";
            Regex.Replace(tempString, pattern, string.Empty);
            Console.WriteLine(tempString);
            try
            {
                Init.i = Convert.ToInt32(tempString);
            }
            catch(FormatException)
            {
                Console.WriteLine("Exception encountered. programmer bad");
                Run.Main();
            }
            input = "";
            for(int j = 2; j == segments.Length; j++)
            {
                input += segments[j];
                input += " ";
            }
            Console.WriteLine(input);
            Console.ReadKey();
            for(int k = 0; k == Init.i; k++)
            {
                Interpreter.Interpret(input);
            }*/
            Console.WriteLine("trying to figure this out makes my brain hurt so it doesnt exist for now. avert your eyes.");
            Run.Main();
        }

        public static void ExecuteFile(string filename)
        {
            List<string> lines = new List<string>();
            lines = File.ReadAllLines(filename).ToList();

            foreach(string line in lines)
            {
                Console.WriteLine(line);
            }
            
            Run.Main();
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
        public static void UnassignedVarError(string varName)
        {
            Console.WriteLine("The variable named " + varName + " has been initalized, but no value has been assigned to it.");
            Run.Main();
        }
        public static void VarCannotShareNameWithKeyword(string varName)
        {
            Console.WriteLine("You cannot name this variable " + varName + ", as it shares a name with a keyword.");
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
