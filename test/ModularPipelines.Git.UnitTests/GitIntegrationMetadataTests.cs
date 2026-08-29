using System.ComponentModel;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Attributes;
using ModularPipelines.Extensions;
using ModularPipelines.Git.Extensions;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.Git.UnitTests;

public class GitIntegrationMetadataTests
{
    [Test]
    public async Task LegacyContextAccessorIsHiddenAndObsolete()
    {
        var accessor = typeof(GitExtensions).GetMethod("Git")!;

        using (Assert.Multiple())
        {
            await Assert.That(accessor.GetCustomAttribute<EditorBrowsableAttribute>()!.State)
                .IsEqualTo(EditorBrowsableState.Never);
            await Assert.That(accessor.GetCustomAttribute<ObsoleteAttribute>()!.Message)
                .IsEqualTo("Use context.Tools.Get<IGit>().");
        }
    }

    [Test]
    public async Task GeneratedMetadataRegistersGitServices()
    {
        await Assert.That(
                typeof(GitExtensions).Assembly
                    .GetCustomAttributes<ModularPipelinesContextAttribute>())
            .HasSingleItem();

        await using var pipeline = await TestPipelineBuilder.Create()
            .AddModule<TrueModule>()
            .BuildAsync();

        pipeline.Services.GetRequiredService<IGit>();
    }
}
