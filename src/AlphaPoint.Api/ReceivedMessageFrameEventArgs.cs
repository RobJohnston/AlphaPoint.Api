using AlphaPoint.Api.Models;
using System;

namespace AlphaPoint.Api
{
    /// <summary>
    /// Provides data for the <see cref="ReceivedMessageFrame"/> event.
    /// </summary>
    public class ReceivedMessageFrameEventArgs : EventArgs
    {
        /// <summary>
        /// Creates a new <see cref="ReceivedMessageFrameEventArgs"/> with the specified values.
        /// </summary>
        /// <param name="frame">The message frame received.</param>
        public ReceivedMessageFrameEventArgs(MessageFrame frame)
        {
            MessageFrameResponse = frame;
        }

        /// <summary>
        /// Gets the message frame that was received.
        /// </summary>
        public MessageFrame MessageFrameResponse { get; private set; }
    }
}