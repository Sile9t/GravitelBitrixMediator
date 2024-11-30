namespace Entities.Exceptions
{
    public abstract class BadRequestException : Exception
    {
        protected BadRequestException(string message)
            : base(message) 
        { }
    }

    public sealed class CallInfoBadRequestException : BadRequestException
    {
        public CallInfoBadRequestException()
            : base(@"GravitelClientCallInfo sent from client was null!")
        {
            
        }
    }

    public sealed class ClientPhoneBadRequestException : BadRequestException
    {
        public ClientPhoneBadRequestException()
            : base(@"ClientPhone sent from client was null!")
        {
            
        }
    }

    public sealed class UserPhoneBadRequestException : BadRequestException
    {
        public UserPhoneBadRequestException()
            : base(@"UserPhone sent from client was null!")
        {
            
        }
    }

    public sealed class CommandErrorException : BadRequestException
    {
        public CommandErrorException(string message)
            : base(message)
        {
            
        }
    }
}
