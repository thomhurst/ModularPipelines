using TUnit.Core;
using TUnit.Core.Interfaces;

namespace ModularPipelines.UnitTests.Attributes;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public class LinuxOnlyTestAttribute : Attribute, ITestAttribute
{
    /// <inheritdoc/>
    public Task ApplyToTest(TestContext testContext)
    {
        if (!OperatingSystem.IsLinux())
        {
            testContext.SkipTest("Linux only test");
        }

        return Task.CompletedTask;
    }
}