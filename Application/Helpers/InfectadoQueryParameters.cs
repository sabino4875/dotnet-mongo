namespace Api.CoronaVirusStatistics.Application.Helpers
{
    using System;

    public class InfectadoQueryParameters
    {
        public InfectadoQueryParameters(Int32? pageNumber, Int32? pageSize, String sexo) 
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            Sexo = sexo;
        }

        public Int32? PageNumber { get; private set; }
        public Int32? PageSize { get; private set; }
        public String Sexo { get; private set; }
    }
}
