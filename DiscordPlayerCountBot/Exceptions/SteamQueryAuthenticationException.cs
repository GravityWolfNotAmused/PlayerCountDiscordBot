namespace PlayerCountBot.Exceptions
{
    internal class RconAuthenticationException : Exception
    {
        public RconAuthenticationException(string? message) : base(message)
        {
        }
    }
}
