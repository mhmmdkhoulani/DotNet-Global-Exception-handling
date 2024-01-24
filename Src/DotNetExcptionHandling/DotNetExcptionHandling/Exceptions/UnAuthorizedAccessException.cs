namespace DotNetExcptionHandling.Exceptions
{
    public class UnAuthorizedAccessException : Exception
    {
        public UnAuthorizedAccessException(string message) : base(message)
        {
        }
    }
}
