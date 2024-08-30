namespace ScheduleCronJobs.Services
{

    /// <summary>
    /// https://github.com/lukencode/FluentEmail
    /// </summary>
    public class EmailService : IFluentEmail
    {
        private IEmailSender _sender;

        public IFluentEmail Body(string body)
        {
            throw new NotImplementedException();
        }

        public IFluentEmail From(string email)
        {
            throw new NotImplementedException();
        }


        public IFluentEmail Subject(string subject)
        {
            throw new NotImplementedException();
        }

        public IFluentEmail To(string email)
        {
            throw new NotImplementedException();
        }


        public Task SendAsync(CancellationToken token)
        {
            return _sender.SendAsync(this, token);
        }
    }

    public class EmailConfiguration { }

    public interface IEmailConfiguration
    {
        IEmailSender AddCongiration(EmailConfiguration configuration);
    }

    public interface IEmailSender
    {
    
        Task SendAsync(IFluentEmail email,CancellationToken token);
    }

    public interface IFluentEmail
    {
        IFluentEmail From(string email);

        IFluentEmail To(string email);

        IFluentEmail Subject(string subject);

        IFluentEmail Body(string body);

        Task SendAsync(CancellationToken token);

    }
}
