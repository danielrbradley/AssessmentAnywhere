namespace AssessmentAnywhere.Services.Users
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class EmailAddressDuplicateException : Exception
    {
        public EmailAddressDuplicateException()
        {
        }

        public EmailAddressDuplicateException(string message)
            : base(message)
        {
        }

        public EmailAddressDuplicateException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected EmailAddressDuplicateException(
            SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}