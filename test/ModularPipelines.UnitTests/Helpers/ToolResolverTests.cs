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

    [Test]
    public async Task ResolveTool_RuntimeValue_OverridesAttribute()
    {
        var resolver = new ToolResolver();
        var options = new DirectToolOptions { Tool = "runtime-tool" };

        var tool = resolver.ResolveTool(options);

        await Assert.That(tool).IsEqualTo("runtime-tool");
    }

    [Test]
    public async Task ResolveTool_DerivedAttribute_OverridesBaseAttribute()
    {
        var resolver = new ToolResolver();

        var tool = resolver.ResolveTool(typeof(OverriddenToolOptions));

        await Assert.That(tool).IsEqualTo("npx");
    }

    // Test fixtures
    [CliTool("mytool")]
    internal record DirectToolOptions : CommandLineToolOptions;

    [CliTool("git")]
    internal abstract record GitOptionsBase : CommandLineToolOptions;

    internal record InheritedToolOptions : GitOptionsBase;

    [CliTool("npx")]
    private record OverriddenToolOptions : GitOptionsBase;

    [CliTool("docker")]
    internal abstract record DockerOptionsBase : CommandLineToolOptions;

    internal abstract record DockerContainerOptions : DockerOptionsBase;

    internal record DeeplyInheritedOptions : DockerContainerOptions;

    internal record NoToolOptions : CommandLineToolOptions;
}
