using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers;

public class DockerCliCompatibilityTests
{
    [Test]
    public async Task SupportedAliases_IncludeAlias_WhenCanonicalCommandWasScraped()
    {
        var aliases = DockerCliCompatibility.GetSupportedCommandGroupAliases(
            [CreateCommand("buildx")]);

        var alias = aliases.Single();
        await Assert.That(alias.Alias).IsEqualTo("builder");
        await Assert.That(alias.CanonicalCommand).IsEqualTo("buildx");
    }

    [Test]
    public async Task SupportedAliases_ExcludeAlias_WhenCanonicalCommandWasNotScraped()
    {
        var aliases = DockerCliCompatibility.GetSupportedCommandGroupAliases(
            [CreateCommand("container")]);

        await Assert.That(aliases).IsEmpty();
    }

    private static CliCommandDefinition CreateCommand(string commandGroup)
    {
        return new CliCommandDefinition
        {
            FullCommand = $"docker {commandGroup}",
            CommandParts = [commandGroup],
            ClassName = $"Docker{commandGroup}Options",
            ParentClassName = "DockerOptions",
            ToolNamespacePrefix = "Docker",
            Options = [],
        };
    }
}
