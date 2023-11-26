namespace API.Errors;

public class ApiResponse
{
    public ApiResponse(int statusCode, string message = null)
    {
        StatusCode = statusCode;
        Message = message ?? GetDefaultMessage(statusCode);
    }

    public int StatusCode { get; set; }

    public string Message { get; set; }

    private string GetDefaultMessage(int statusCode)
    {
        return statusCode switch
        {
            StatusCodes.Status400BadRequest => "A bad request, you have made",
            StatusCodes.Status401Unauthorized => "Authorized, you are not",
            StatusCodes.Status404NotFound => "Resource found, it was not",
            StatusCodes.Status500InternalServerError => "Error are the path to the dark side. Errors lead to anger. Anger leads to hate. Hate leads to career change",
            _ => null,
        };
    }
}