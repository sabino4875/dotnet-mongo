namespace Api.CoronaVirusStatistics.EndPoints
{
    using Microsoft.AspNetCore.Builder;
    using FluentValidation;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Api.CoronaVirusStatistics.Application.Services;
    using System;
    using MongoDB.Driver;
    using Api.CoronaVirusStatistics.Domain.Entities;
    using Api.CoronaVirusStatistics.Application.DTO;
    using Api.CoronaVirusStatistics.Application.Helpers;
    using Newtonsoft.Json;

    public static class InfectadoEndPoints
    {
        public static void MapInfectadoEndPoints(this WebApplication app)
        {
            var defaultUri = "api/v1/infectados";

            //GET: ?page={page?}&pagesize={pagesize?}&sexo={sexo?}
            app.MapGet(defaultUri, async ([FromQuery] Int32? pageNumber, [FromQuery] Int32? pageSize, [FromQuery] String sexo, [FromServices] IInfectadoService service, HttpResponse response) =>
            {
                try
                {
                    var result = await service.ListAll(criteria: new InfectadoQueryParameters(pageNumber, pageSize, sexo));

                    var paginationMetadata = new
                    {
                        result.TotalItems,
                        result.PageSize,
                        result.CurrentPage,
                        result.TotalPages,
                        result.HasNextPage,
                        result.HasPreviousPage
                    };

                    response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(paginationMetadata));

                    return Results.Ok(result);
                }
                catch (Exception ex)
                {
                    return Results.Problem(statusCode: 500, title: "infectado list", detail: ex.Message);
                }
            })
            .Produces<InfectadoPagedList>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status500InternalServerError);


            //GET: /{id}
            app.MapGet(String.Concat(defaultUri, "/{id}"), async (String id, [FromServices] IInfectadoService service) =>
            {
                try
                {
                    Guid vId;
                    if (Guid.TryParse(id, out vId))
                    {
                        var exists = await service.Exists(criteria: Builders<Infectado>.Filter.Eq(e => e.EntityId, vId));
                        if (exists)
                        {
                            var result = await service.FindOne(vId);
                            return Results.Ok(result);
                        }

                        return Results.NotFound(new { message = "Registro não localizado." });
                    }
                    return Results.BadRequest(new { message = "Id informado inválido." });
                }
                catch (Exception ex)
                {
                    return Results.Problem(statusCode: 500, title: "infectado find", detail: ex.Message);
                }
            })
            .Produces<InfectadoDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError);

            //POST: /
            app.MapPost(defaultUri, async ([FromBody] InfectadoDto entity, [FromServices] IInfectadoService service,
                [FromServices] IValidator<InfectadoDto> validator) =>
            {
                try
                {
                    if (entity != null)
                    {
                        var validation = await validator.ValidateAsync(entity);
                        if (!validation.IsValid)
                        {
                            return Results.ValidationProblem(validation.ToDictionary());
                        }

                        var id = await service.Insert(entity);
                        return Results.Created(String.Concat(defaultUri, $"/{id}"), entity);
                    }
                    return Results.BadRequest(new { message = "Por favor, preencha todos os campos." });
                }
                catch (Exception ex)
                {
                    return Results.Problem(statusCode: 500, title: "infectado insert", detail: ex.Message);
                }
            })
            .Accepts<InfectadoDto>("application/json")
            .Produces<InfectadoDto>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError)
            .ProducesProblem(StatusCodes.Status500InternalServerError, "application/json")
            .ProducesValidationProblem(StatusCodes.Status500InternalServerError);


            //PUT: /{id}
            app.MapPut(String.Concat(defaultUri,"/{id}"), async (String id, [FromBody] InfectadoDto entity, [FromServices] IInfectadoService service,
                [FromServices] IValidator<InfectadoDto> validator) =>
            {
                try
                {
                    Guid vId;

                    if(Guid.TryParse(id, out vId))
                    {
                        if (entity != null)
                        {
                            var exists = await service.Exists(Builders<Infectado>.Filter.Eq(e => e.EntityId, vId));

                            if(!exists)
                            {
                                return Results.NotFound(new { message = "Registro não localizado."});
                            }

                            var validation = await validator.ValidateAsync(entity);
                            if (!validation.IsValid)
                            {
                                return Results.ValidationProblem(validation.ToDictionary());
                            }

                            var result = await service.Update(vId, entity);
                            return Results.Ok(result);
                        }
                        return Results.BadRequest(new { message = "Por favor, preencha todos os campos." });
                    }
                    return Results.BadRequest(new { message = "Id informado inválido." });
                }
                catch (Exception ex)
                {
                    return Results.Problem(statusCode: 500, title: "infectado update", detail: ex.Message);
                }
            })
            .Accepts<InfectadoDto>("application/json")
            .Produces<InfectadoDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError);

            //DELETE: /{id}
            app.MapDelete(String.Concat(defaultUri, "/{id}"), async (String id, [FromServices] IInfectadoService service) =>
            {
                try
                {
                    Guid vId;

                    if (Guid.TryParse(id, out vId))
                    {
                        var exists = await service.Exists(Builders<Infectado>.Filter.Eq(e => e.EntityId, vId));

                        if (!exists)
                        {
                            return Results.NotFound(new { message = "Registro não localizado." });
                        }

                        var result = await service.Delete(vId);
                        return Results.Ok(result);
                    }
                    return Results.BadRequest(new { message = "Id informado inválido." });
                }
                catch (Exception ex)
                {
                    return Results.Problem(statusCode: 500, title: "infectado delete", detail: ex.Message);
                }
            })
            .Produces<InfectadoDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError);
        }
    }
}
