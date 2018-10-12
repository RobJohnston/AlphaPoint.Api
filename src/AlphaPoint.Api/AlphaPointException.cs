using System;
using System.Runtime.Serialization;
using AlphaPoint.Api.Models;

namespace AlphaPoint.Api
{
    [Serializable]
    internal class AlphaPointException : Exception
    {
        private MessageFrame messageFrameResponse;

        public AlphaPointException()
        {
        }

        //public AlphaPointException(MessageFrame messageFrameResponse)
        //{
        //    this.messageFrameResponse = messageFrameResponse;
        //}

        public AlphaPointException(string message) : base(message)
        {
        }

        public AlphaPointException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected AlphaPointException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public string FunctionName { get; internal set; }
        public long Sequence { get; internal set; }
        //public int Code { get; internal set; }
    }
}