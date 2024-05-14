using TUnit.Core.Exceptions;
using TUnit.Core.Interfaces;

namespace ModularPipelines.UnitTests.Attributes;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public class WindowsOnlyTestAttribute : Attribute, IApplicableTestAttribute
{
    /// <inheritdoc/>
    public Task Apply(TestContext testContext)
    {
        if (!OperatingSystem.IsWindows())
        {
            throw new SkipTestException("Windows only test");
        }

        return Task.CompletedTask;
    }
}