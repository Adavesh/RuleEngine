using System;

namespace RuleEngine
{
    public class SignalReceiver : ISignalReceiver
    {
        private RuleManager _RuleManager;

        /// <summary>
        /// Initializes new instance.
        /// </summary>
        /// <param name="ruleManager"></param>
        public SignalReceiver(RuleManager ruleManager)
        {
            _RuleManager = ruleManager;
        }

        /// <summary>
        /// This method is invoked when a signal received.
        /// </summary>
        /// <param name="signal">Incoming signal</param>
        public void ReceiveSignal(Signal signal)
        {
            bool isSignalValid = _RuleManager.ValidateSignal(signal);

            if (!isSignalValid)
            {
                Console.WriteLine(signal.SourceID);
            }
        }
    }

}
