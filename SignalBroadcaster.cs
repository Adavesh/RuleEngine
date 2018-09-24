using System.Collections.Generic;

namespace RuleEngine
{
    /// <summary>
    /// Entity that represents a signal broadcaster.
    /// </summary>
    public class SignalBroadcaster : ISignalBroadcaster
    {
        private List<ISignalReceiver> _Receivers;

        /// <summary>
        /// Initiallize the broadcaster instance.
        /// </summary>
        public SignalBroadcaster()
        {
            _Receivers = new List<ISignalReceiver>();
        }

        /// <summary>
        /// Register the reciever to receive the Signal
        /// </summary>
        /// <param name="receiver">Instance of <see cref="ISignalReceiver"/>.</param>
        public void RegisterSignalReceiver(ISignalReceiver receiver)
        {
            _Receivers.Add(receiver);
        }

        /// <summary>
        /// UnRegister the reciever from receiving the Signal
        /// </summary>
        /// <param name="receiver">Instance of <see cref="ISignalReceiver"/>.</param>
        public void UnregisterSignalReceiver(ISignalReceiver receiver)
        {
            _Receivers.Remove(receiver);
        }

        /// <summary>
        /// Notifies all the recievers that a signal is arrived.
        /// </summary>
        /// <param name="signal">Incoming signal</param>
        public void Broadcast(Signal signal)
        {
            foreach (var receiver in _Receivers)
            {
                receiver.ReceiveSignal(signal);
            }
        }
    }
}
