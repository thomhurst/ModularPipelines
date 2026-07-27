using ModularPipelines.OptionsGenerator.Models;

namespace ModularPipelines.OptionsGenerator.Generators;

/// <summary>
/// Curated runnable documentation examples. Scraped help can describe syntax, but it
/// cannot establish whether a command is interactive, destructive, or safe to copy.
/// </summary>
internal static class DocumentationExampleCatalog
{
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

    public static CliToolDefinition Apply(CliToolDefinition tool)
    {
        if (!Tools.TryGetValue(tool.ToolName, out var toolMetadata))
        {
            return tool;
        }

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
