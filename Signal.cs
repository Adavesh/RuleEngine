using System;

namespace RuleEngine
{
    /// <summary>
    /// Entity that represents incoming signal
    /// </summary>
    public class Signal
    {
        /// <summary>
        /// Get the source id of the signal
        /// </summary>
        public string SourceID { get; private set; }

        /// <summary>
        /// Get the value of the signal
        /// </summary>
        public string Value { get; private set; }
        
        /// <summary>
        /// Get the type of value in the signal
        /// </summary>
        public string ValueType { get; private set; }

        /// <summary>
        /// Initializes new instance
        /// </summary>
        /// <param name="sourceId">Source ID of the signal</param>
        /// <param name="value">Value of the signal</param>
        /// <param name="valueType">Type of the signal value</param>
        public Signal(string sourceId, string value, string valueType)
        {
            SourceID = sourceId?.Trim();
            Value = value?.Trim();
            ValueType = valueType?.Trim();
        }

        /// <summary>
        /// Implicity conversion method to convert Json string to Signal instance.
        /// </summary>
        /// <param name="signalData">Instance of Signal class.</param>
        public static implicit operator Signal(string signalData)
        {
            if (string.IsNullOrWhiteSpace(signalData))
            {
                throw new FormatException("Signal Data Empty");
            }

            var signal = new Signal(string.Empty, string.Empty, string.Empty);

            var signalTokens = signalData.Trim('{').Trim('}').
                Replace("'", string.Empty).Replace("\"", string.Empty).
                Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var token in signalTokens)
            {
                var separatorIndex = token.IndexOf(':');
                if (separatorIndex <0)
                {
                    throw new FormatException("Signal data has incorrect format");
                }
                var dataName = token.Substring(0, separatorIndex).Trim().ToUpperInvariant();
                var dataValue = token.Substring(separatorIndex+1).Trim();
                switch (dataName)
                {
                    case "SIGNAL":
                        signal.SourceID = dataValue;
                        break;

                    case "VALUE":
                        signal.Value = dataValue;
                        break;

                    case "VALUE_TYPE":
                        signal.ValueType = dataValue;
                        break;
                }
            }

            return signal;
        }
    }
}
