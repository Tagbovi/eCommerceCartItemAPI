using ShoppingCart.Controllers;

namespace ShoppingCart.LoggingError
{
    public interface ILoggerWrapper
    {
        void LogError(Exception exception, string message, params object[] args);
    }

    public class LoggerWrapper : ILoggerWrapper
    {
        private readonly ILogger<CartController> logger;

        public LoggerWrapper(ILogger<CartController> logger)
        {
            this.logger = logger;
        }

        public void LogError(Exception exception, string message, params object[] args)
        {
            logger.LogError(exception, message, args);
        }
    }
}
