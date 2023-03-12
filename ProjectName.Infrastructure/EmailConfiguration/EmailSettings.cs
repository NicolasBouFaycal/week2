namespace ProjectName.Infrastructure.EmailConfiguration
{
    public class EmailSettings
    {
        public string EmailId { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public bool UseSSL { get; set; }

        public EmailSettings(string emailId, string name, string password, string host, int port, bool useSsl)
        {
            EmailId = emailId;
            Name = name;
            Password = password;
            Host = host;
            Port = port;
            UseSSL = useSsl;
        }
    }
}
