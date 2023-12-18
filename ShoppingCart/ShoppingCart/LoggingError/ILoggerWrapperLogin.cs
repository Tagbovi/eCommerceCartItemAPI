using ShoppingCart.Controllers;

namespace ShoppingCart.LoggingError
{
    public interface ILoggerWrapperLogin
    {


        void LogError(Exception exception, string message, params object[] args);
    }

        public class LoggerWrapperLogin : ILoggerWrapperLogin
        {
        private readonly ILogger<CartUserLoginController> logger;

        public LoggerWrapperLogin(ILogger<CartUserLoginController> logger)
            {
                this.logger = logger;
            }

            public void LogError(Exception exception, string message, params object[] args)
            {
                logger.LogError(exception, message, args);
            }
        }
    
}
