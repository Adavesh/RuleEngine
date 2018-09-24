using System;
using System.Collections.Generic;
using System.IO;

namespace RuleEngine
{
    class Program
    {
        static void Main(string[] args)
        {
            ConsoleColor consoleForegroundColor = Console.ForegroundColor;
            var ruleManager = new RuleManager();            

            try
            {
                string action = args[0];
                string actionData = args[1];

                switch (action.ToUpperInvariant())
                {
                    case "ADD":
                        try
                        {
                            ruleManager.AddRule(actionData);
                        }
                        catch (FormatException excep)
                        {
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                            Console.WriteLine("\nError: Rule should in the format {signal:<source>, condition:<condition>, target_value:<value> }");
                            Console.WriteLine(excep.Message);
                        }
                        break;

                    case "REMOVE":
                        try
                        {
                            ruleManager.RemoveRule(actionData);
                        }
                        catch (FormatException)
                        {
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                            Console.WriteLine("\nError: Rule should in the format {signal:<source>, condition:<condition>, target_value:<value> }");
                        }
                        break;

                    case "EXECUTE":
                        if (File.Exists(actionData))
                        {
                            var jsonText = File.ReadAllText(actionData);
                            var signals = DeserializeRuleJson(jsonText);

                            var signalReciever = new SignalReceiver(ruleManager);
                            var broadcaster = new SignalBroadcaster();
                            broadcaster.RegisterSignalReceiver(signalReciever);

                            foreach (var signal in signals)
                            {
                                broadcaster.Broadcast(signal);
                            }
                        }
                        break;
                }
            }
            catch (Exception)
            {
                ShowUsage();
            }
            finally
            {
                Console.ForegroundColor = consoleForegroundColor;
            }
        }


        /// <summary>
        /// Convert the Json string into List<Signal> objects
        /// </summary>
        private static List<Signal> DeserializeRuleJson(string json)
        {
            var signals = new List<Signal>();

            json = json.Trim(new[] { '[', ']' });

            int startCount = 0;
            for (int i = 0; i < json.Length; i++)
            {
                if (json[i] == '{')
                {
                    startCount = i;
                }
                else if (json[i] == '}')
                {
                    string instanceJson = json.Substring(startCount, i - startCount + 1);
                    signals.Add(instanceJson);
                }

            }

            return signals;
        }

        /// <summary>
        /// Shows how to use the program on command prompt.
        /// </summary>
        static void ShowUsage()
        {
            Console.WriteLine("\nUSAGE:\n");

            Console.WriteLine("RuleEngine Add <ruledata>    : Adds new rule with given ruledata");
            Console.WriteLine("RuleEngine Remove <ruledata> : Remove new rule with given ruledata");
            Console.WriteLine("RuleEngine Execute <File>    : Executes the rules against all signals specified in the file");
        }
    }
}
