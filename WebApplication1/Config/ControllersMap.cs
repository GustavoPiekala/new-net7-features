using API.Models;
using API.Services;
using Microsoft.OpenApi.Models;

namespace API.Extensions
{
    public static class ControllersMap
    {
        public static void Map(WebApplication app)
        {
            //OpenApi
            app.MapGet("/openapi/{text}", (string text) =>
            {
                return $"The parameter is {text}";
            })
            .WithName("GetOpenApi")
            .WithOpenApi(x =>
            {
                x.Description = "This is the endpoint description";
                x.Summary = "This is the endpoint summary";
                x.Parameters[0].Description = "Parameter that will be return";
                x.Parameters[0].AllowEmptyValue = false;

                x.Responses = new OpenApiResponses
                {
                    [StatusCodes.Status200OK.ToString()] = new OpenApiResponse
                    {
                        Description = "Returns the URI input"
                    },
                    [StatusCodes.Status400BadRequest.ToString()] = new OpenApiResponse
                    {
                        Description = "Invalid input"
                    }
                };

                return x;
            });

            //DI
            app.MapGet("/dependecyinjection", (IDotNet7DependencyInjectionService service) =>
            {
                return service.GetResponse();
            })
            .WithName("GetDependecyInjection")
            .WithOpenApi();

            //Cache
            app.MapGet("/simplecache", () =>
            {
                return $"This is a 20 secods cache - {DateTime.Now}";
            })
            .WithName("GetSimpleCache")
            .WithOpenApi()
            .CacheOutput(x => x.Expire(TimeSpan.FromSeconds(20)));

            app.MapGet("/varybycache", (string? text) =>
            {
                return $"This is a vary by query params cache - {text} - {DateTime.Now}";
            })
            .WithName("GetVaryByCache")
            .WithOpenApi()
            .CacheOutput(x => x.SetVaryByQuery("text").Expire(TimeSpan.FromSeconds(20)));

            //Rate Limiting
            app.MapGet("/ratelimit", () =>
            {
                return $"This is a 10 seconds fixed window rate limiting request";
            })
            .WithName("GetRateLimit")
            .WithOpenApi()
            .RequireRateLimiting("testpolicy");

            //Raw string literal
            app.MapGet("/rawstringliteral", () =>
            {
                return GetRawString();
            })
            .WithName("GetRawStringLiteral")
            .WithOpenApi();

            //Required modifier
            app.MapGet("/requiredmodifier", () =>
            {
                return GetRequiredModifier();
            })
            .WithName("GetRequiredModifier")
            .WithOpenApi();

            //List patterns
            app.MapGet("/listpatterns", () =>
            {
                return GetListPatterns();
            })
            .WithName("GetListPatterns")
            .WithOpenApi();

            app.MapGet("/listpatterns2", () =>
            {
                return GetListPatterns2();
            })
            .WithName("GetListPatterns2")
            .WithOpenApi();
        }

        private static object GetListPatterns2()
        {
            return ListPatterns.GetUserInfo();
        }

        private static object GetListPatterns()
        {
            int[] summer = { 12, 1, 2, 3 };
            int[] fall = { 3, 4, 5, 6 };
            int[] winter = { 6, 7, 8, 9 };
            int[] spring = { 9, 10, 11, 12 };
            int[] invalidMonth = { 0 };
            int[] invalidMonth2 = { 13 };
            int[] summerAhead = { 1, 2 };
            int[] randomMonths = { 12, 1, 3, 5, 10, 4 };

            var results = new List<string>
            {
                ListPatterns.GetSeasons(summer),
                ListPatterns.GetSeasons(fall),
                ListPatterns.GetSeasons(winter),
                ListPatterns.GetSeasons(spring),
                ListPatterns.GetSeasons(invalidMonth),
                ListPatterns.GetSeasons(invalidMonth2),
                ListPatterns.GetSeasons(summerAhead),
                ListPatterns.GetSeasons(randomMonths)
            };

            return results;
        }

        private static RequiredModifier[] GetRequiredModifier() 
        {

            //var requiredModifier = new RequiredModifier();
            //requiredModifier.Name = "a";
            //requiredModifier.LastName = "b";

            var requiredMembersByConstructor = new RequiredModifier("ByConstructorName", "ByConstructorLastName");
            var requiredMembersByObjectPropsInitialize = new RequiredModifier() { Name = "ByPropsInitName", LastName = "ByPropsInitLastName" };
            return new RequiredModifier[] { requiredMembersByConstructor, requiredMembersByObjectPropsInitialize };
        } 

        private static string GetRawString()
        {
            var text = "raw string";

            return $"""
                    This is a "{text}" example
                        Here is a new indented line.
                    """;
        }
    }
}
