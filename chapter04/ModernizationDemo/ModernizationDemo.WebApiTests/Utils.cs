using ModernizationDemo.WebApi.Client;

namespace ModernizationDemo.WebApiTests
{
    public static class Utils
    {
        public static async Task AssertException(TestEnvironment environment, string message, Func<Task> action)
        {
            var ex = await Assert.ThrowsAsync<ApiException>(action);
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