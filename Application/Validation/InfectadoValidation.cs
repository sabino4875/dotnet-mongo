namespace Api.CoronaVirusStatistics.Application.Validation
{
    using Api.CoronaVirusStatistics.Application.DTO;
    using FluentValidation;
    using System;
    using System.Collections.Generic;

    public class InfectadoValidation : AbstractValidator<InfectadoDto>
    {
        public InfectadoValidation()
        {
            var validSexo = new List<String> { "M", "F" };
            RuleFor(e => e.Sexo).Cascade(CascadeMode.Stop).NotNull()
                .WithMessage("O campo sexo deve ser informado.")
                .Must(v => validSexo.Contains(v))
                .WithMessage($"Por favor, informe um dos valores para o campo sexo: {String.Join(" ou ", validSexo)}");
            RuleFor(e => e.DataNascimento).Cascade(CascadeMode.Stop).NotNull()
                .WithMessage("O campo nascimento deve ser informado.")
                .LessThan(DateTime.Now.AddYears(-16)).WithMessage("Entrevistado deve ter no mímnimo 16 anos");
            RuleFor(e => e.Latitude).NotNull().WithMessage("O campo latitude deve ser informado.");
            RuleFor(e => e.Longitude).NotNull().WithMessage("O campo longitude deve ser informado.");
        }
    }
}
