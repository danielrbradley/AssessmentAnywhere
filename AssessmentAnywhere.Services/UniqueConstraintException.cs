namespace AssessmentAnywhere.Services
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class UniqueConstraintException : Exception
    {
        public UniqueConstraintException()
        {
        }

        public UniqueConstraintException(string message)
            : base(message)
        {
        }

        public UniqueConstraintException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected UniqueConstraintException(
            SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}