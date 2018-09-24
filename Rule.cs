using System;
using System.Linq;

namespace RuleEngine
{
    /// <summary>
    /// Entity for holding Rule data.
    /// </summary>
    public class Rule
    {
        /// <summary>
        /// Get the Signal source.
        /// </summary>
        public string SignalSource { get; private set; }

        /// <summary>
        /// Get the condition of the rule.
        /// </summary>
        public string Condition { get; private set; }

        /// <summary>
        /// Get the value to validate
        /// </summary>
        public string TargetValue { get; private set; }

        /// <summary>
        /// Get the type of the value to validate
        /// </summary>
        public string ValueType { get; private set; }

        /// <summary>
        /// Initializes new instance of Rule.
        /// </summary>
        /// <param name="signal">Source of the signal</param>
        /// <param name="condition">Condition of the rule</param>
        /// <param name="targetValue">Rule value to validate against signal value.</param>
        public Rule(string signal, string condition, string targetValue, string valueType)
        {
            if (string.IsNullOrWhiteSpace(signal) || string.IsNullOrWhiteSpace(condition)
                || string.IsNullOrWhiteSpace(valueType) ||
                (!valueType.Equals("String", StringComparison.OrdinalIgnoreCase) && string.IsNullOrWhiteSpace(targetValue)))
            {
                throw new FormatException("Invalid rule specified. Please check the input fields and values.");
            }

            SignalSource = signal?.Trim();
            Condition = condition?.Trim();
            TargetValue = targetValue?.Trim();
            ValueType = valueType?.Trim();
        }

        /// <summary>
        /// Implicit convertion method to convert Json string to Rule instance.
        /// </summary>
        /// <param name="ruleData">Instance of <see cref="Rule"/>.</param>
        public static implicit operator Rule(string ruleData)
        {
            if (string.IsNullOrWhiteSpace(ruleData))
            {
                throw new FormatException("Empty rule not allowed");
            }

            var ruleTokens = ruleData.Trim(new[] { '{', '}' }).
                Replace("'", string.Empty).Replace("\"", string.Empty).
                Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            string signalSource = string.Empty;
            string condition = string.Empty;
            string targetValue = string.Empty;
            string valueType = string.Empty;

            foreach (var token in ruleTokens)
            {
                var data = token.Split(new[] { ':' }, StringSplitOptions.RemoveEmptyEntries);

                switch (data[0].Trim().ToLowerInvariant())
                {
                    case "signal":
                        signalSource = data[1].Trim();
                        break;

                    case "condition":
                        condition = data[1].Trim();
                        if (!IsConditionValid(condition))
                        {
                            throw new FormatException("Unrecognize rule condition specified.");
                        }
                        break;

                    case "target_value":
                        targetValue = data[1].Trim();
                        break;

                    case "value_type":
                        valueType = data[1].Trim();
                        if (!ValidateValueType(valueType))
                        {
                            throw new FormatException("Unrecognize value_type specified.");
                        }
                        break;
                }
            }

            Rule rule = new Rule(signalSource, condition, targetValue, valueType);

            return rule;
        }

        /// <summary>
        /// Check whether the condition specified within the rule is supported.
        /// </summary>
        /// <param name="codition">Condition string.</param>
        /// <returns>True if the condition is valid.</returns>
        private static bool IsConditionValid(string codition)
        {
            string[] validConditions = new[] { "=", "!=", ">", ">=", "<", "<=" };
            return validConditions.Any(condition => condition.Equals(codition.Trim(), StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Check whether the value type is valid
        /// </summary>
        private static bool ValidateValueType(string type)
        {
            return type.Equals("String", StringComparison.OrdinalIgnoreCase) ||
                type.Equals("Integer", StringComparison.OrdinalIgnoreCase) ||
                type.Equals("DateTime", StringComparison.OrdinalIgnoreCase);
        }
    }
}
