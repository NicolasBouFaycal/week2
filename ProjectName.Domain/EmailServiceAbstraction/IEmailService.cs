namespace ProjectName.Domain.EmailServiceAbstraction
{
    public interface IEmailService
    {
        bool SendEmail(string EmailToId, string EmailToName, string EmailSubject, string EmailBody);
    }
}
