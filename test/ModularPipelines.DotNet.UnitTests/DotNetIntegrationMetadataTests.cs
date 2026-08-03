using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Attributes;
using ModularPipelines.DotNet.Extensions;
using ModularPipelines.DotNet.Services;
using ModularPipelines.Extensions;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.DotNet.UnitTests;

public class DotNetIntegrationMetadataTests
{
    [Test]
    public async Task GeneratedMetadataRegistersDotNetServices()
    {
        await Assert.That(
                typeof(DotNetExtensions).Assembly
                    .GetCustomAttributes<ModularPipelinesContextAttribute>())
            .HasSingleItem();

        await using var pipeline = await TestPipelineBuilder.Create()
            .AddModule<TrueModule>()
            .BuildAsync();

        pipeline.Services.GetRequiredService<IDotNet>();
    }
}
