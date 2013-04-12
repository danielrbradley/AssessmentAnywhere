namespace AssessmentAnywhere.Services.Users
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class IncorrectPasswordException : Exception
    {
        public IncorrectPasswordException()
        {
        }

        public IncorrectPasswordException(string message)
            : base(message)
        {
        }

        public IncorrectPasswordException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected IncorrectPasswordException(
            SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}