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
            "brew",
            "choco",
            "cosign",
            "eksctl",
            "flux",
            "flyway",
            "gradle",
            "grype",
            "hadolint",
            "kind",
            "kustomize",
            "liquibase",
            "minikube",
            "mvn",
            "podman",
            "pulumi",
            "shellcheck",
            "skopeo",
            "snyk",
            "sonar-scanner",
            "syft",
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
            ["az"] = new(
                "az account list",
                [
                    Safe("az account list"),
                ]),
            ["buildah"] = new(
                "buildah containers",
                [
                    Unsafe("buildah add", isDestructive: true),
                    Safe("buildah containers"),
                ]),
            ["cargo"] = new(
                "cargo check",
                [
                    Safe(
                        "cargo check",
                        ("Quiet", "true")),
                ]),
            ["docker"] = new(
                "docker info",
                [
                    Safe("docker info"),
                ]),
            ["dotnet"] = new(
                "dotnet workload list",
                [
                    Safe("dotnet workload list"),
                ]),
            ["gcloud"] = new(
                "gcloud info",
                [
                    Safe(
                        "gcloud info",
                        ("Anonymize", "true")),
                ]),
            ["gh"] = new(
                "gh config list",
                [
                    Safe("gh config list"),
                ]),
            ["git"] = new(
                "git status",
                [
                    Safe(
                        "git status",
                        ("Short", "true")),
                ]),
            ["go"] = new(
                "go vet",
                [
                    Safe("go vet"),
                ]),
            ["helm"] = new(
                "helm env",
                [
                    Safe("helm env"),
                ]),
            ["jq"] = new(
                "jq",
                [
                    Safe(
                        "jq",
                        ("Filter", "\".\""),
                        ("InputFiles", "[\"input.json\"]")),
                ]),
            ["kubectl"] = new(
                "kubectl config view",
                [
                    Safe("kubectl config view"),
                ]),
            ["newman"] = new(
                null,
                [
                    Unsafe("newman run", isDestructive: true),
                ]),
            ["nbgv"] = new(
                "nbgv get-version",
                [
                    Safe("nbgv get-version"),
                ]),
            ["npm"] = new(
                "npm audit",
                [
                    Safe(
                        "npm audit",
                        ("AuditLevel", "\"high\"")),
                ]),
            ["packer"] = new(
                null,
                [
                    Unsafe("packer console", isInteractive: true),
                ]),
            ["pip"] = new(
                "pip freeze",
                [
                    Safe("pip freeze"),
                ]),
            ["pnpm"] = new(
                "pnpm audit",
                [
                    Safe(
                        "pnpm audit",
                        ("AuditLevel", "\"high\"")),
                ]),
            ["terraform"] = new(
                "terraform validate",
                [
                    Safe("terraform validate"),
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
