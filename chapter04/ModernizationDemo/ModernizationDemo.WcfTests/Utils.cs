using System;
using System.ServiceModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ModernizationDemo.WcfTests
{
    public static class Utils
    {
        public static void AssertException(TestEnvironment environment, string message, Action action)
        {
            var ex = Assert.ThrowsException<FaultException<ExceptionDetail>>(action);
            ValidateExceptionMessage(message, ex);
        }

        public static void ValidateExceptionMessage(string message, Exception ex)
        {
            if (ex.Message.Contains(message))
            {
                return;
            }
            else if (ex.InnerException != null)
            {
                ValidateExceptionMessage(message, ex.InnerException);
            }
            else
            {
                Assert.Fail($"Exception message does not contain '{message}'");
            }
        }
    }
}