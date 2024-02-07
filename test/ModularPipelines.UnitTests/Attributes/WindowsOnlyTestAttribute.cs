using TUnit.Core.Interfaces;

namespace ModularPipelines.UnitTests.Attributes;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public class WindowsOnlyTestAttribute : Attribute, ITestAttribute
{
    /// <inheritdoc/>
    public Task ApplyToTest(TestContext testContext)
    {
        if (!OperatingSystem.IsWindows())
        {
            testContext.SkipTest("Windows only test");
        }

        return Task.CompletedTask;
    }
}