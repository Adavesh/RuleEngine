using System;

namespace RuleEngine
{
    public delegate void SignalReceivedEventHandler(SignalReceivedEventArgs e);

    /// <summary>
    /// Provides data for SignalReceived event handler
    /// </summary>
    public class SignalReceivedEventArgs : EventArgs
    {
        /// <summary>
        /// Get the signal that has been received.
        /// </summary>
        public Signal Signal { get; private set; }

        /// <summary>
        /// Initialize the instance.
        /// </summary>
        /// <param name="incomingSignal">Incoming <see cref="Signal"/> instance.</param>
        public SignalReceivedEventArgs(Signal incomingSignal)
        {
            this.Signal = Signal;
        }
    }
}
