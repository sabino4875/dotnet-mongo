namespace Api.CoronaVirusStatistics.InfraStructure.Repositories
{
    using Api.CoronaVirusStatistics.Domain.Entities;
    using Api.CoronaVirusStatistics.Domain.Repositories;
    using Api.CoronaVirusStatistics.InfraStructure.Context;

    public class InfectadoRepository : Repository<Infectado>, IInfectadoRepository
    {
        public InfectadoRepository(ApplicationDBContext context) : base(context, "infectados")
        {
        }
    }
}
