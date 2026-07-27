using ModularPipelines.OptionsGenerator.Models;

namespace ModularPipelines.OptionsGenerator.Generators;

/// <summary>
/// Tool-specific executable prerequisites layered over a safe generic fallback.
/// </summary>
internal static class ExecutablePrerequisiteCatalog
{
    private static readonly IReadOnlyDictionary<string, CliExecutablePrerequisite> Prerequisites =
        new Dictionary<string, CliExecutablePrerequisite>(StringComparer.OrdinalIgnoreCase)
        {
            ["sonar-scanner"] = new()
            {
                CommandName = "sonar-scanner",
                SupportedVersion = "8.0.1.6346",
                InstallationUrl =
                    "https://docs.sonarsource.com/sonarqube-server/analyzing-source-code/scanners/sonarscanner",
                InstallationNotes =
                    "The generator workflow downloads the Linux x64 SonarScanner CLI distribution.",
            },
            ["syft"] = new()
            {
                CommandName = "syft",
                InstallationUrl = "https://oss.anchore.com/docs/installation/syft/",
            },
            ["terraform"] = new()
            {
                CommandName = "terraform",
                InstallationUrl = "https://developer.hashicorp.com/terraform/install",
            },
        };

    public static CliToolDefinition Apply(CliToolDefinition tool)
    {
        if (tool.ExecutablePrerequisite is not null
            || !string.IsNullOrWhiteSpace(tool.ExecutablePrerequisiteMetadataExemption))
        {
            return tool;
        }

        var prerequisite = Prerequisites.GetValueOrDefault(tool.ToolName)
                           ?? new CliExecutablePrerequisite
                           {
                               CommandName = tool.ToolName,
                               InstallationNotes =
                                   "Follow the executable's official documentation for installation instructions.",
                           };

        return tool with { ExecutablePrerequisite = prerequisite };
    }
}
