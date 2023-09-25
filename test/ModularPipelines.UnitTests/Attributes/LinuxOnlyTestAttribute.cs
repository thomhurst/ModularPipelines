using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;

namespace ModularPipelines.UnitTests.Attributes;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public class LinuxOnlyTestAttribute : Attribute, IApplyToTest
{
    /// <inheritdoc/>
    public void ApplyToTest(Test test)
    {
        if (!OperatingSystem.IsLinux())
        {
            Assert.Ignore("This test is only runnable on Linux");
        }
    }
}