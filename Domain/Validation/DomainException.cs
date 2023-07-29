namespace Api.CoronaVirusStatistics.Domain.Entities
{
    using System;
    public class DomainException : Exception
    {
        public DomainException(String message) : base(message) 
        { }

        public static void When(String message, Boolean show)
        {
            if(show) 
            {
                throw new DomainException(message);
            }
        }
    }
}
