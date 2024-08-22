using System;
using System.Web.Services.Protocols;
using Grpc.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ModernizationDemo.SoapTests
{
    public static class Utils
    {
        public static void AssertException(TestEnvironment environment, string message, Action action)
        {
            Exception ex;
            if (environment == TestEnvironment.Soap)
            {
                ex = Assert.ThrowsException<SoapException>(action);
            }
            else if (environment == TestEnvironment.SoapCore)
            {
                ex = Assert.ThrowsException<SoapHeaderException>(action);
            }
            else if (environment == TestEnvironment.Grpc)
            {
                ex = Assert.ThrowsException<RpcException>(action);
                return;
            }
            else
            {
                throw new NotSupportedException();
            }

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