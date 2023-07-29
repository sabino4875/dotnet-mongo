namespace Api.CoronaVirusStatistics.IoC
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Diagnostics;
    using Microsoft.AspNetCore.Http;
    using System;

    using System.IO;
    using System.Net;
    using Newtonsoft.Json;

    public static class ApplicationCollectionExtensions
    {
        private static IApplicationBuilder MapCors(this IApplicationBuilder app)
        {
            app.UseCors(p =>
            {
                p.WithOrigins("http://localhost:3000")
                .AllowAnyHeader()
                .AllowCredentials()
                .WithMethods("GET", "POST", "PATCH", "DELETE");
            });
            return app;
        }

        private static IApplicationBuilder MapResponseHeaders(this IApplicationBuilder app)
        {
            app.Use(async (context, next) =>
            {
                context.Response.Headers.Add("accept-ranges", "bytes");
                context.Response.Headers.Add("access-control-allow-origin", "*");
                context.Response.Headers.Add("connection", "keep-alive");
                context.Response.Headers.Add("cross-origin-embedder-policy", "credentialless");
                context.Response.Headers.Add("cross-origin-opener-policy", "same-origin");
                context.Response.Headers.Add("cross-origin-resource-policy", "cross-origin");
                context.Response.Headers.Add("date", DateTimeOffset.Now.ToUniversalTime().ToString("r"));//Thu, 08 Jun 2023 22:24:55 GMT
                context.Response.Headers.Add("keep-alive", "timeout=5");
                context.Response.Headers.Add("access-control-allow-headers", "Content-Type, X-CSRF-Token, X-Requested-With, Accept, Accept-Version, Accept-Encoding, Content-Length, Content-MD5, Date, X-Api-Version, X-File-Name");
                context.Response.Headers.Add("access-control-allow-credentials", "true");
                await next();
            });
            return app;
        }

        private static IApplicationBuilder MapExtensions(this IApplicationBuilder app)
        {
            app.UseStatusCodePages();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            return app;
        }

        private static IApplicationBuilder MapExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (Int32)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        await context.Response.WriteAsync(new ErrorDetails()
                        {
                            StatusCode = context.Response.StatusCode,
                            Message = contextFeature.Error.Message,
                            InnerException = contextFeature.Error.InnerException,
                        }.ToString());
                    }
                });
            });
            app.UseHsts();
            return app;
        }

        public static IApplicationBuilder MapInfraStructure(this IApplicationBuilder app)
        {
            //exception configuration
            app.MapExceptionHandler();
            //cors configuration
            app.MapCors();
            //response headers configuration
            app.MapResponseHeaders();
            //extensions configuration
            app.MapExtensions();
            return app;
        }
    }
    public class ErrorDetails
    {
        public Int32 StatusCode { get; set; }
        public String Message { get; set; }
        public Exception InnerException { get;set; }
        public override String ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
