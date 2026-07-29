using ModularPipelines.OptionsGenerator.Generators;
using ModularPipelines.OptionsGenerator.Models;

namespace ModularPipelines.OptionsGenerator.Tests.Generators;

public class CommandGroupAliasGenerationTests
{
    private static readonly CliCommandGroupAlias BuilderAlias = new()
    {
        Alias = "builder",
        CanonicalCommand = "buildx",
        ObsoleteMessage = "Use Buildx instead.",
    };

    [Test]
    public async Task Generators_Emit_One_Canonical_Tree_With_Compatibility_Wrappers()
    {
        var tool = CreateTool();

        var optionFiles = await new OptionsClassGenerator().GenerateAsync(tool);
        var serviceFiles = await new SubDomainClassGenerator().GenerateAsync(tool);
        var serviceInterface = (await new ServiceInterfaceGenerator().GenerateAsync(tool))
            .Single().Content;
        var serviceImplementation =
            (await new ServiceImplementationGenerator().GenerateAsync(tool))
            .Single().Content;
        var registration = (await new DependencyRegistrationGenerator().GenerateAsync(tool))
            .Single().Content;

        var builderOptions = optionFiles.Single(file =>
            file.RelativePath.EndsWith(
                "DockerBuilderBuildOptions.Generated.cs",
                StringComparison.Ordinal));
        await Assert.That(builderOptions.Content)
            .Contains("public record DockerBuilderBuildOptions : DockerBuildxBuildOptions;");
        await Assert.That(builderOptions.Content).DoesNotContain("[CliOption(");
        await Assert.That(optionFiles).DoesNotContain(file =>
            file.RelativePath.EndsWith(
                "DockerBuilderOptions.Generated.cs",
                StringComparison.Ordinal));

        var canonicalService = serviceFiles.Single(file =>
            Path.GetFileName(file.RelativePath).Equals(
                "DockerBuildx.Generated.cs",
                StringComparison.Ordinal));
        await Assert.That(canonicalService.Content)
            .Contains("public class DockerBuildx : IDockerBuildx, IDockerBuilder");

        var builderService = serviceFiles.Single(file =>
            Path.GetFileName(file.RelativePath).Equals(
                "DockerBuilder.Generated.cs",
                StringComparison.Ordinal));
        await Assert.That(builderService.Content)
            .Contains("public class DockerBuilder : DockerBuildx");
        await Assert.That(builderService.Content).DoesNotContain("Task<CommandResult>");

        var builderInterface = serviceFiles.Single(file =>
            file.RelativePath.EndsWith(
                "IDockerBuilder.Generated.cs",
                StringComparison.Ordinal));
        await Assert.That(builderInterface.Content)
            .Contains("public interface IDockerBuilder : IDockerBuildx");

        await Assert.That(serviceInterface)
            .Contains("IDockerBuilder Builder { get; }");
        await Assert.That(serviceImplementation)
            .Contains("public IDockerBuilder Builder => (IDockerBuilder)Buildx;");
        await Assert.That(registration)
            .Contains("services.TryAddScoped<IDockerBuilder>");
    }

    private static CliToolDefinition CreateTool() =>
        new()
        {
            ToolName = "docker",
            NamespacePrefix = "Docker",
            TargetNamespace = "ModularPipelines.Docker",
            OutputDirectory = "src/ModularPipelines.Docker",
            CommandGroupAliases = [BuilderAlias],
            Commands =
            [
                Command(["buildx"], "DockerBuildxOptions", null),
                Command(["buildx", "build"], "DockerBuildxBuildOptions", "Buildx"),
            ],
        };

    private static CliCommandDefinition Command(
        string[] commandParts,
        string className,
        string? subDomainGroup) =>
        new()
        {
            FullCommand = $"docker {string.Join(' ', commandParts)}",
            CommandParts = commandParts,
            ClassName = className,
            ParentClassName = "DockerOptions",
            ToolNamespacePrefix = "Docker",
            Options = [],
            SubDomainGroup = subDomainGroup,
        };
}
