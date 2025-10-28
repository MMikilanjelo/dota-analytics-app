using Microsoft.AspNetCore.Http;
using SharedKernel;
using SharedKernel.Errors;

namespace Modules.ExternalAccounts.Infrastructure.Extensions;

public static class CustomResults
{
    public static IResult Problem(Result result)
    {
        if (result.IsSuccess)
        {
            throw new InvalidOperationException();
        }

        return Results.Problem(
            title: result.DomainError.Code,
            detail: result.DomainError.Message,
            type: GetType(),
            statusCode: 500,
            extensions: GetErrors(result));

        static string GetType() =>
            "https://tools.ietf.org/html/rfc7231#section-6.6.1";

        // static int GetStatusCode(ErrorType errorType) =>
        //     errorType switch
        //     {
        //         ErrorType.Validation or ErrorType.Problem => StatusCodes.Status400BadRequest,
        //         ErrorType.NotFound => StatusCodes.Status404NotFound,
        //         ErrorType.Conflict => StatusCodes.Status409Conflict,
        //         _ => StatusCodes.Status500InternalServerError
        //     };

        static Dictionary<string, object?>? GetErrors(Result result)
        {
            if (result.DomainError is not ValidationError validationError)
            {
                return null;
            }

            return new Dictionary<string, object?>
            {
                { "errors", validationError.Errors }
            };
        }
    }
}