using dotMailify.Smtp.Config;

namespace dotMailify.Smtp
{
    public class FromConfigSmtpClientProvider : SmtpClientProvider
    {
        public FromConfigSmtpClientProvider() : base(new FromConfigSmtpClientProviderSettings()) { }
    }
}