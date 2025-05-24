namespace APBD_8.Exceptions;

public class BadRequestException : ApiException
{
    public BadRequestException(string message) : base(StatusCodes.Status400BadRequest, message)
    {
    }
}