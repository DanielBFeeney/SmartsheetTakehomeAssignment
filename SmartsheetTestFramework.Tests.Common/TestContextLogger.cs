using System;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SmartsheetTestFramework.Tests.Common
{
    public class TestContextLogger
    {
        TestContext Context { get; set; }
        public TestContextLogger(TestContext context)
        {
            Context = context;
        }

        /// <summary>
        /// This method will write the message to the 
        /// log along with the corresponding severity
        /// </summary>
        /// <param name="message">The message to write to the log</param>
        /// <param name="severity">The severity of the log entry</param>
        public void Write(string message, LogEntrySeverityEnum severity)
        {
            try
            {
                write(message, severity);
            }
            catch (Exception)
            {
                // Log failures should not 
                // cause our program to crash
            }
        }

        /// <summary>
        /// This method will write the message to the log
        /// along with select attributes from the exception object
        /// </summary>
        /// <param name="message">The message to write to the log</param>
        /// <param name="severity">The severity of the event</param>
        /// <param name="exception">The exception object</param>
        public void Write(string message, LogEntrySeverityEnum severity, Exception exception)
        {
            try
            {
                write(format(message, exception), severity);
            }
            catch (Exception)
            {
                // Log failures should not 
                // cause our program to crash
            }
        }

        /// <summary>
        /// Writes the actual data to the log.
        /// </summary>
        /// <param name="message">The text to log</param>
        /// <param name="severity">The severity of the log entry</param>
        private void write(string message, LogEntrySeverityEnum severity)
        {
            Context.WriteLine("[" + DateTime.Now + "]" + message);
        }

        /// <summary>
        /// Helper method that will format the log text
        /// and include selected attributes from the exception
        /// object
        /// </summary>
        /// <param name="message">The message text</param>
        /// <param name="exception">The exception object</param>
        /// <returns></returns>
        private string format(string message, Exception exception)
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendFormat("{0}\r\n", message);

            builder.AppendFormat(
                "Exception Type: {0}\r\n\r\n" +
                "Exception Message: \r\n{1}\r\n\r\n" +
                "Stack Trace:\r\n{2}",
                exception.GetType().ToString(),
                exception.Message,
                exception.StackTrace);

            if (null != exception.InnerException)
            {
                builder.AppendFormat(
                "InnerException Type: {0}\r\n\r\n" +
                "InnerException Message: \r\n{1}\r\n\r\n" +
                "InnerException Stack Trace:\r\n{2}",
                exception.InnerException.GetType().ToString(),
                exception.InnerException.Message,
                exception.InnerException.StackTrace);
            }

            return builder.ToString();
        }

    }
}
