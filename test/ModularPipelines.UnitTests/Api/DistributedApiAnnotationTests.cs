using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using ModularPipelines.Distributed;

namespace ModularPipelines.UnitTests.Api;

public class DistributedApiAnnotationTests
{
    [Test]
    public async Task Action_Overload_Warns_Trim_And_Aot_Callers()
    {
        var method = typeof(DistributedPipelineBuilderExtensions).GetMethod(
            nameof(DistributedPipelineBuilderExtensions.AddDistributedMode),
            [typeof(PipelineBuilder), typeof(Action<DistributedOptions>)]);

        using (Assert.Multiple())
        {
            await Assert.That(method).IsNotNull();
            await Assert.That(method!.GetCustomAttribute<RequiresUnreferencedCodeAttribute>())
                .IsNotNull();
            await Assert.That(method.GetCustomAttribute<RequiresDynamicCodeAttribute>())
                .IsNotNull();
        }
    }
}
