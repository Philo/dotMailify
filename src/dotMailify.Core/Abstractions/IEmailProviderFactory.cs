namespace dotMailify.Core.Abstractions
{
    public interface IEmailProviderFactory
    {
        IEmailProvider GetEmailProvider();
    }
}