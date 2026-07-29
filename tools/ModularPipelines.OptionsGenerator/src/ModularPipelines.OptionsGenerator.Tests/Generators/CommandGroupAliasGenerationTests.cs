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
        var rootBuilderOptions = optionFiles.Single(file =>
            file.RelativePath.EndsWith(
                "DockerBuilderOptions.Generated.cs",
                StringComparison.Ordinal));
        await Assert.That(rootBuilderOptions.Content)
            .Contains("public record DockerBuilderOptions : DockerBuildxOptions;");
        var historyLogsOptions = optionFiles.Single(file =>
            file.RelativePath.EndsWith(
                "DockerBuilderHistoryLogsOptions.Generated.cs",
                StringComparison.Ordinal));
        await Assert.That(historyLogsOptions.Content)
            .Contains("public new DockerBuilderHistoryLogsProgress? Progress");
        await Assert.That(historyLogsOptions.Content)
            .Contains(": (DockerBuilderHistoryLogsProgress)(int)base.Progress.Value;");
        await Assert.That(historyLogsOptions.Content)
            .Contains(": (DockerBuildxHistoryLogsProgress)(int)value.Value;");
        await Assert.That(historyLogsOptions.Content)
            .Contains("[CliOption(\"--progress\", Format = OptionFormat.EqualsSeparated)]");

        var canonicalService = serviceFiles.Single(file =>
            Path.GetFileName(file.RelativePath).Equals(
                "DockerBuildx.Generated.cs",
                StringComparison.Ordinal));
        await Assert.That(canonicalService.Content)
            .Contains("public class DockerBuildx : IDockerBuildx");
        await Assert.That(canonicalService.Content)
            .DoesNotContain("IDockerBuilder");

        var builderService = serviceFiles.Single(file =>
            Path.GetFileName(file.RelativePath).Equals(
                "DockerBuilder.Generated.cs",
                StringComparison.Ordinal));
        await Assert.That(builderService.Content)
            .Contains("public class DockerBuilder : DockerBuildx, IDockerBuilder");
        await Assert.That(builderService.Content)
            .Contains("public virtual Task<CommandResult> Execute(");
        await Assert.That(builderService.Content)
            .Contains("DockerBuilderOptions? options = null");
        await Assert.That(builderService.Content)
            .Contains("return base.Execute(options, executionOptions, cancellationToken);");
        await Assert.That(builderService.Content)
            .Contains("public virtual Task<CommandResult> Build(");
        await Assert.That(builderService.Content)
            .Contains("DockerBuilderBuildOptions? options = null");
        await Assert.That(builderService.Content)
            .Contains("return base.Build(options, executionOptions, cancellationToken);");

        var builderInterface = serviceFiles.Single(file =>
            file.RelativePath.EndsWith(
                "IDockerBuilder.Generated.cs",
                StringComparison.Ordinal));
        await Assert.That(builderInterface.Content)
            .Contains("public interface IDockerBuilder");
        await Assert.That(builderInterface.Content)
            .DoesNotContain(": IDockerBuildx");
        await Assert.That(builderInterface.Content)
            .Contains("DockerBuilderOptions? options = null");
        await Assert.That(builderInterface.Content)
            .Contains("DockerBuilderBuildOptions? options = null");

        await Assert.That(serviceInterface)
            .Contains("IDockerBuilder Builder { get; }");
        await Assert.That(serviceImplementation)
            .Contains("IDockerBuilder builder");
        await Assert.That(serviceImplementation)
            .Contains("Builder = builder;");
        await Assert.That(serviceImplementation)
            .Contains("public IDockerBuilder Builder { get; }");
        await Assert.That(registration)
            .Contains("services.TryAddScoped<IDockerBuilder, DockerBuilder>();");
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
                Command(
                    ["buildx", "history", "logs"],
                    "DockerBuildxHistoryLogsOptions",
                    "Buildx",
                    [
                        new CliOptionDefinition
                        {
                            SwitchName = "--progress",
                            PropertyName = "Progress",
                            CSharpType = "DockerBuildxHistoryLogsProgress?",
                            ValueSeparator = "=",
                            EnumDefinition = new CliEnumDefinition
                            {
                                EnumName = "DockerBuildxHistoryLogsProgress",
                                Values = [],
                            },
                        },
                    ]),
            ],
        };

    private static CliCommandDefinition Command(
        string[] commandParts,
        string className,
        string? subDomainGroup,
        IReadOnlyList<CliOptionDefinition>? options = null) =>
        new()
        {
            FullCommand = $"docker {string.Join(' ', commandParts)}",
            CommandParts = commandParts,
            ClassName = className,
            ParentClassName = "DockerOptions",
            ToolNamespacePrefix = "Docker",
            Options = options ?? [],
            SubDomainGroup = subDomainGroup,
        };
}
