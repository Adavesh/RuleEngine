using RuleEngine.Data;
using System;
using System.Collections.Generic;

namespace RuleEngine
{
    /// <summary>
    /// Manages the user defined rules
    /// </summary>
    public class RuleManager
    {
        /// <summary>
        /// Predefined conditions available for creating rules.
        /// </summary>
        public static string[] ValidConditions
        {
            get
            {
                return new[] { "=", "!=", ">", ">=", "<", "<=" };
            }
        }

        /// <summary>
        /// Gets the collection of rules available in the system.
        /// </summary>
        public IList<Rule> Rules
        {
            get
            {
                return DatabaseManager.GetAllRules();
            }
        }

        /// <summary>
        /// Adds the given <see cref="Rule" instance to the system./>
        /// </summary>
        /// <param name="rule">Instance of <see cref="Rule"/> to be added to the system.</param>
        public void AddRule(Rule rule)
        {
            DatabaseManager.AddRule(rule);
        }

        /// <summary>
        /// Converts the given Json to <see cref="Rule" instance and adds to the system./>
        /// </summary>
        /// <param name="ruleJson">Json string of the <see cref="Rule"/></param>
        public void AddRule(string ruleJson)
        {
            DatabaseManager.AddRule(ruleJson);
        }

        /// <summary>
        /// Removes the given <see cref="Rule" from system./>
        /// </summary>
        /// <param name="rule">Rule to be removed.</param>
        public void RemoveRule(Rule rule)
        {
            DatabaseManager.RemoveRule(rule);
        }

        /// <summary>
        /// Converts given Json to <see cref="Rule" instance and removes it from the system./>
        /// </summary>
        /// <param name="ruleJson">Json string of the rule.</param>
        public void RemoveRule(string ruleJson)
        {
            if (ruleJson == "*")
            {
                DatabaseManager.RemoveAllRules();
                return;
            }
            DatabaseManager.RemoveRule(ruleJson);
        }

        /// <summary>
        /// Validates the given <see cref="Signal" against the set of <see cref="Rule" instances/>/>
        /// </summary>
        /// <param name="signal">Signal to be validated</param>
        /// <returns>True if the signal is valid; False Otherwise</returns>
        public bool ValidateSignal(Signal signal)
        {
            bool isSignalValid = true;

            foreach (var rule in Rules)
            {
                isSignalValid = ValidateSignal(signal, rule);
                if (!isSignalValid)
                {
                    break;
                }
            }

            return isSignalValid;
        }

        /// <summary>
        /// Validates the given <see cref="Signal" against the given <see cref="Rule" instance./>/>
        /// </summary>
        private bool ValidateSignal(Signal signal, Rule rule)
        {
            bool isSignalValid = true;

            if (rule.SignalSource.Equals(signal.SourceID, StringComparison.OrdinalIgnoreCase) && 
                rule.ValueType.Equals(signal.ValueType, StringComparison.OrdinalIgnoreCase))
            {
                switch (rule.Condition)
                {
                    case "=":
                    case "!=":
                        isSignalValid = rule.TargetValue.Equals(signal.Value);
                        if(rule.Condition.Equals("!="))
                        {
                            isSignalValid = !isSignalValid;
                        }
                        break;

                    case ">":
                    case ">=":
                        switch (signal.ValueType.ToUpperInvariant())
                        {
                            case "INTEGER":
                                int.TryParse(signal.Value, out int signalValue);
                                int.TryParse(rule.TargetValue, out int targetValue);
                                isSignalValid = rule.Condition.Equals(">=") ? signalValue >= targetValue : signalValue > targetValue;

                                break;

                            case "DATETIME":
                                DateTime.TryParse(signal.Value, out DateTime signalDate);
                                if (!DateTime.TryParse(rule.TargetValue, out DateTime targetDate))
                                {
                                    if (rule.TargetValue.Equals("TODAY", StringComparison.OrdinalIgnoreCase))
                                    {
                                        targetDate = DateTime.Now;
                                    }
                                }
                                isSignalValid = rule.Condition.Equals(">=") ? signalDate >= targetDate : signalDate > targetDate;
                                break;
                        }
                        break;

                    case "<":
                    case "<=":
                        switch (signal.ValueType.ToUpperInvariant())
                        {
                            case "INTEGER":
                                int.TryParse(signal.Value, out int signalValue);
                                int.TryParse(rule.TargetValue, out int targetValue);
                                isSignalValid = rule.Condition.Equals("<=") ? signalValue <= targetValue : signalValue < targetValue;
                                break;

                            case "DATETIME":
                                DateTime.TryParse(signal.Value, out DateTime signalDate);
                                if (!DateTime.TryParse(rule.TargetValue, out DateTime targetDate))
                                {
                                    if (rule.TargetValue.Equals("TODAY", StringComparison.OrdinalIgnoreCase))
                                    {
                                        targetDate = DateTime.Now;
                                    }
                                }
                                isSignalValid = rule.Condition.Equals("<=") ? signalDate <= targetDate : signalDate < targetDate;

                                break;
                        }
                        break;
                }
            }

            return isSignalValid;
        }
    }
}
