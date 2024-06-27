using TUnit.Core.Exceptions;
using TUnit.Core.Interfaces;

namespace ModularPipelines.UnitTests.Attributes;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public class LinuxOnlyTestAttribute : Attribute, IApplicableTestAttribute
{
    /// <inheritdoc/>
    public Task Apply(TestContext testContext)
    {
        if (!OperatingSystem.IsLinux())
        {
            throw new SkipTestException("Linux only test");
        }

        return Task.CompletedTask;
    }
}