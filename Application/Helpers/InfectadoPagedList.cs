namespace Api.CoronaVirusStatistics.Application.Helpers
{
    using Api.CoronaVirusStatistics.Application.DTO;
    using Api.CoronaVirusStatistics.Domain.Entities;
    using Api.CoronaVirusStatistics.Domain.Repositories;
    using Api.CoronaVirusStatistics.InfraStructure.Repositories;
    using AutoMapper;
    using MongoDB.Driver;
    using System;
    using System.Collections.Generic;

    public class InfectadoPagedList
    {
        private String _sexo;
        public Int32 CurrentPage { get; private set; }
        public Int32 TotalPages { get; private set; }
        public Int32 PageSize { get; private set; }
        public Int64 TotalItems { get; private set; }
        public Boolean HasPreviousPage => CurrentPage > 1;
        public Boolean HasNextPage => CurrentPage < TotalPages;
        public IEnumerable<InfectadoDto> Items { get; private set; }

        public InfectadoPagedList(IInfectadoRepository repository, IMapper mapper, InfectadoQueryParameters criteria)
        {
            CurrentPage = 1;
            PageSize = 10;
            if (criteria.PageNumber.HasValue) CurrentPage = criteria.PageNumber.Value;
            if (CurrentPage < 0) CurrentPage = 1;
            if (criteria.PageSize.HasValue) PageSize = criteria.PageSize.Value;
            if (PageSize < 10) PageSize = 10;
            if (PageSize > 50) PageSize = 50;

            _sexo = "T";
            var valid = new List<String> { "M", "F" };
            if (!String.IsNullOrEmpty(criteria.Sexo) && !String.IsNullOrWhiteSpace(criteria.Sexo) && valid.Contains(criteria.Sexo.ToUpper()))
            {
                _sexo = criteria.Sexo.ToUpper();
            }

            var filter = Builders<Infectado>.Filter.Empty;
            if (_sexo != "T")
            {
                filter = Builders<Infectado>.Filter.Eq(e => e.Sexo, _sexo);
            }

            TotalItems = repository.Count(filter).Result;

            TotalPages = Convert.ToInt32(Math.Ceiling((Decimal)TotalItems / (Decimal)PageSize));
            if (TotalPages < 1) TotalPages = 1;
            if (CurrentPage > TotalPages) CurrentPage = TotalPages;
            var offset = (CurrentPage - 1) * PageSize;

            Items = mapper.Map<IEnumerable<InfectadoDto>>(repository.ListAll(filter, PageSize, offset).Result);
        }
    }
}
