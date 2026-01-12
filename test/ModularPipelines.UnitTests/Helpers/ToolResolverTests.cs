using ModularPipelines.Attributes;
using ModularPipelines.Helpers.Internal;
using ModularPipelines.Options;

namespace ModularPipelines.UnitTests.Helpers;

public class ToolResolverTests
{
    [Test]
    public async Task ResolveTool_FromDirectAttribute_ReturnsTool()
    {
        var resolver = new ToolResolver();

        var tool = resolver.ResolveTool(typeof(DirectToolOptions));

        await Assert.That(tool).IsEqualTo("mytool");
    }

    [Test]
    public async Task ResolveTool_FromInheritedAttribute_ReturnsTool()
    {
        var resolver = new ToolResolver();

        var tool = resolver.ResolveTool(typeof(InheritedToolOptions));

        await Assert.That(tool).IsEqualTo("git");
    }

    [Test]
    public async Task ResolveTool_FromDeeplyInheritedAttribute_ReturnsTool()
    {
        var resolver = new ToolResolver();

        var tool = resolver.ResolveTool(typeof(DeeplyInheritedOptions));

        await Assert.That(tool).IsEqualTo("docker");
    }

    [Test]
    public async Task ResolveTool_NoAttribute_ReturnsNull()
    {
        var resolver = new ToolResolver();

        var tool = resolver.ResolveTool(typeof(NoToolOptions));

        await Assert.That(tool).IsNull();
    }

    [Test]
    public async Task ResolveTool_FromInstance_ReturnsToolFromAttribute()
    {
        var resolver = new ToolResolver();
        var options = new DirectToolOptions();

        var tool = resolver.ResolveTool(options);

        await Assert.That(tool).IsEqualTo("mytool");
    }

    // Test fixtures
    [CliTool("mytool")]
    private record DirectToolOptions : CommandLineToolOptions;

    [CliTool("git")]
    private abstract record GitOptionsBase : CommandLineToolOptions;

    private record InheritedToolOptions : GitOptionsBase;

    [CliTool("docker")]
    private abstract record DockerOptionsBase : CommandLineToolOptions;

    private abstract record DockerContainerOptions : DockerOptionsBase;

    private record DeeplyInheritedOptions : DockerContainerOptions;

    private record NoToolOptions : CommandLineToolOptions;
}
