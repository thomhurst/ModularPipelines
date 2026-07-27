using ModularPipelines.OptionsGenerator.Models;

namespace ModularPipelines.OptionsGenerator.Generators;

/// <summary>
/// Curated runnable documentation examples. Scraped help can describe syntax, but it
/// cannot establish whether a command is interactive, destructive, or safe to copy.
/// </summary>
internal static class DocumentationExampleCatalog
{
    // These tools intentionally receive the non-runnable service-resolution example
    // until a command and all required sample values have been explicitly reviewed.
    private static readonly IReadOnlySet<string> ToolsWithoutCuratedExamples =
        new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "argocd",
            "aws",
            "az",
            "brew",
            "cargo",
            "choco",
            "cosign",
            "docker",
            "dotnet",
            "eksctl",
            "flux",
            "flyway",
            "gcloud",
            "gh",
            "git",
            "go",
            "gradle",
            "grype",
            "hadolint",
            "helm",
            "kind",
            "kubectl",
            "kustomize",
            "liquibase",
            "minikube",
            "mvn",
            "pip",
            "pnpm",
            "podman",
            "pulumi",
            "shellcheck",
            "skopeo",
            "snyk",
            "sonar-scanner",
            "syft",
            "terraform",
            "trivy",
            "winget",
            "yarn",
            "yq",
        };

    private static readonly IReadOnlyDictionary<string, ToolMetadata> Tools =
        new Dictionary<string, ToolMetadata>(StringComparer.OrdinalIgnoreCase)
        {
            ["ansible"] = new(
                "ansible",
                [
                    Safe(
                        "ansible",
                        ("Pattern", "\"localhost\""),
                        ("ListHosts", "true")),
                ]),
            ["buildah"] = new(
                "buildah containers",
                [
                    Unsafe("buildah add", isDestructive: true),
                    Safe("buildah containers"),
                ]),
            ["jq"] = new(
                "jq",
                [
                    Safe(
                        "jq",
                        ("Filter", "\".\""),
                        ("InputFiles", "[\"input.json\"]")),
                ]),
            ["newman"] = new(
                null,
                [
                    Unsafe("newman run", isDestructive: true),
                    Unsafe("newman URL"),
                ]),
            ["nbgv"] = new(
                "nbgv get-version",
                [
                    Safe("nbgv get-version"),
                ]),
            ["packer"] = new(
                null,
                [
                    Unsafe("packer console", isInteractive: true),
                ]),
            ["vault"] = new(
                "vault status",
                [
                    Unsafe("vault delete", isDestructive: true),
                    Safe("vault status"),
                ]),
        };

    public static void ValidateRegisteredTools(IEnumerable<string> toolNames)
    {
        ArgumentNullException.ThrowIfNull(toolNames);

        var missingPolicies = toolNames
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .Where(toolName =>
                !Tools.ContainsKey(toolName)
                && !ToolsWithoutCuratedExamples.Contains(toolName))
            .OrderBy(toolName => toolName, StringComparer.OrdinalIgnoreCase)
            .ToArray();

        if (missingPolicies.Length > 0)
        {
            throw new InvalidOperationException(
                "Documentation example policy is missing for registered tool(s): "
                + string.Join(", ", missingPolicies)
                + ". Add a curated example or explicitly omit the tool.");
        }
    }

    public static CliToolDefinition Apply(CliToolDefinition tool)
    {
        if (!Tools.TryGetValue(tool.ToolName, out var toolMetadata))
        {
            if (ToolsWithoutCuratedExamples.Contains(tool.ToolName))
            {
                return tool with
                {
                    PreferredDocumentationExampleCommand = null,
                };
            }

            return tool;
        }

        ValidateMetadata(tool, toolMetadata);

        var commands = tool.Commands
            .Select(command => Apply(command, toolMetadata))
            .ToArray();

        return tool with
        {
            Commands = commands,
            PreferredDocumentationExampleCommand =
                tool.PreferredDocumentationExampleCommand
                ?? toolMetadata.PreferredCommand,
        };
    }

    private static void ValidateMetadata(CliToolDefinition tool, ToolMetadata toolMetadata)
    {
        var actualCommands = tool.Commands
            .Select(command => command.FullCommand)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);
        var missingCommands = toolMetadata.Commands
            .Select(command => command.FullCommand)
            .Where(command => !actualCommands.Contains(command))
            .ToArray();

        if (missingCommands.Length > 0)
        {
            throw new InvalidOperationException(
                $"Documentation example catalog for '{tool.ToolName}' references missing command(s): "
                + string.Join(", ", missingCommands)
                + ". Update the catalog to match the scraper output.");
        }

        if (toolMetadata.PreferredCommand is not null
            && !actualCommands.Contains(toolMetadata.PreferredCommand))
        {
            throw new InvalidOperationException(
                $"Documentation example catalog for '{tool.ToolName}' prefers missing command "
                + $"'{toolMetadata.PreferredCommand}'. Update the catalog to match the scraper output.");
        }
    }

    private static CliCommandDefinition Apply(
        CliCommandDefinition command,
        ToolMetadata toolMetadata)
    {
        var commandMetadata = toolMetadata.Commands.FirstOrDefault(metadata =>
            string.Equals(metadata.FullCommand, command.FullCommand, StringComparison.OrdinalIgnoreCase));
        if (commandMetadata is null)
        {
            return command;
        }

        return command with
        {
            IsSafeForDocumentation = commandMetadata.IsSafeForDocumentation,
            IsInteractive = commandMetadata.IsInteractive,
            IsDestructive = commandMetadata.IsDestructive,
            DocumentationExampleValues = commandMetadata.Values,
        };
    }

    private static CommandMetadata Safe(
        string fullCommand,
        params (string PropertyName, string CSharpExpression)[] values) =>
        new(
            fullCommand,
            IsSafeForDocumentation: true,
            IsInteractive: false,
            IsDestructive: false,
            values.ToDictionary(
                value => value.PropertyName,
                value => value.CSharpExpression,
                StringComparer.Ordinal));

    private static CommandMetadata Unsafe(
        string fullCommand,
        bool isInteractive = false,
        bool isDestructive = false) =>
        new(
            fullCommand,
            IsSafeForDocumentation: false,
            isInteractive,
            isDestructive,
            new Dictionary<string, string>(StringComparer.Ordinal));

    private sealed record ToolMetadata(
        string? PreferredCommand,
        IReadOnlyList<CommandMetadata> Commands);

    private sealed record CommandMetadata(
        string FullCommand,
        bool IsSafeForDocumentation,
        bool IsInteractive,
        bool IsDestructive,
        IReadOnlyDictionary<string, string> Values);
}
