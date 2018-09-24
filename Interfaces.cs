using System;

namespace RuleEngine
{

    /// <summary>
    /// Interface for signal receiever classes
    /// </summary>
    public interface ISignalReceiver
    {
        /// <summary>
        /// This method is invoked by broadcaster
        /// </summary>
        /// <param name="signal">Incoming signal</param>
        void ReceiveSignal(Signal signal);
    }

    /// <summary>
    /// Interface for signal broadcaster classes
    /// </summary>
    public interface ISignalBroadcaster
    {
        void RegisterSignalReceiver(ISignalReceiver receiver);

        void UnregisterSignalReceiver(ISignalReceiver receiver);

        void Broadcast(Signal signal);
    }
}
