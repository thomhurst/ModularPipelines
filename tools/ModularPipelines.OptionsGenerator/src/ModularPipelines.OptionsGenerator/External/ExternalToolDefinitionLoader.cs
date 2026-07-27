using System.Text.Json;
using System.Text.Json.Serialization;
using ModularPipelines.OptionsGenerator.Models;

namespace ModularPipelines.OptionsGenerator.External;

/// <summary>
/// Versioned document accepted by the external generator invocation.
/// </summary>
public sealed record ExternalToolDefinitionDocument
{
    public const int CurrentSchemaVersion = 1;

    public required int SchemaVersion { get; init; }

    public required CliToolDefinition Tool { get; init; }
}

/// <summary>
/// Loads and validates external CLI metadata without assuming the ModularPipelines repository layout.
/// </summary>
public static class ExternalToolDefinitionLoader
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        ReadCommentHandling = JsonCommentHandling.Skip,
        AllowTrailingCommas = true,
        Converters =
        {
            new JsonStringEnumConverter(JsonNamingPolicy.CamelCase, allowIntegerValues: false),
        },
    };

    public static async Task<CliToolDefinition> LoadAsync(
        string metadataPath,
        string outputDirectory,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(metadataPath);
        ArgumentException.ThrowIfNullOrWhiteSpace(outputDirectory);

        await using var stream = File.OpenRead(Path.GetFullPath(metadataPath));
        ExternalToolDefinitionDocument document;
        try
        {
            document = await JsonSerializer.DeserializeAsync<ExternalToolDefinitionDocument>(
                    stream,
                    JsonOptions,
                    cancellationToken)
                ?? throw new InvalidDataException("External tool metadata is empty.");
        }
        catch (JsonException exception)
        {
            throw new InvalidDataException(
                $"External tool metadata '{metadataPath}' is invalid: {exception.Message}",
                exception);
        }

        if (document.SchemaVersion != ExternalToolDefinitionDocument.CurrentSchemaVersion)
        {
            throw new InvalidDataException(
                $"Unsupported external tool metadata schemaVersion {document.SchemaVersion}. "
                + $"Expected {ExternalToolDefinitionDocument.CurrentSchemaVersion}.");
        }

        var tool = document.Tool
            ?? throw new InvalidDataException("External tool metadata must define tool.");
        Validate(tool, outputDirectory);
        return tool;
    }

    public static void Validate(CliToolDefinition tool, string outputDirectory)
    {
        ArgumentNullException.ThrowIfNull(tool);
        ArgumentException.ThrowIfNullOrWhiteSpace(outputDirectory);

        RequireValue(tool.ToolName, "tool.toolName");
        RequireValue(tool.NamespacePrefix, "tool.namespacePrefix");
        RequireValue(tool.TargetNamespace, "tool.targetNamespace");
        RequireValue(tool.OutputDirectory, "tool.outputDirectory");

        if (tool.Commands is not { Count: > 0 })
        {
            throw new InvalidDataException("External tool metadata must define at least one command.");
        }

        RequireFileNameComponent(tool.NamespacePrefix, "tool.namespacePrefix");
        ValidateRelativeOutputPath(tool.OutputDirectory, outputDirectory, "tool.outputDirectory");
        if (!string.IsNullOrWhiteSpace(tool.DocumentationOutputDirectory))
        {
            ValidateRelativeOutputPath(
                tool.DocumentationOutputDirectory,
                outputDirectory,
                "tool.documentationOutputDirectory");
        }

        foreach (var command in tool.Commands)
        {
            RequireValue(command.FullCommand, "tool.commands[].fullCommand");
            RequireFileNameComponent(command.ClassName, "tool.commands[].className");
            RequireFileNameComponent(command.ParentClassName, "tool.commands[].parentClassName");
            RequireFileNameComponent(command.ToolNamespacePrefix, "tool.commands[].toolNamespacePrefix");
            if (!string.Equals(
                    command.ToolNamespacePrefix,
                    tool.NamespacePrefix,
                    StringComparison.Ordinal))
            {
                throw new InvalidDataException(
                    "tool.commands[].toolNamespacePrefix must match tool.namespacePrefix.");
            }

            command.ValidateOperandCoverage();
        }
    }

    private static void ValidateRelativeOutputPath(
        string relativePath,
        string outputDirectory,
        string propertyName)
    {
        if (Path.IsPathRooted(relativePath))
        {
            throw new InvalidDataException($"{propertyName} must be relative to --output-dir.");
        }

        var root = Path.GetFullPath(outputDirectory);
        var candidate = Path.GetFullPath(Path.Combine(root, relativePath));
        var relativeCandidate = Path.GetRelativePath(root, candidate);
        if (Path.IsPathRooted(relativeCandidate)
            || relativeCandidate == ".."
            || relativeCandidate.StartsWith($"..{Path.DirectorySeparatorChar}", StringComparison.Ordinal)
            || relativeCandidate.StartsWith($"..{Path.AltDirectorySeparatorChar}", StringComparison.Ordinal))
        {
            throw new InvalidDataException($"{propertyName} must stay within --output-dir.");
        }
    }

    private static void RequireValue(string value, string propertyName)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new InvalidDataException($"{propertyName} is required.");
        }
    }

    private static void RequireFileNameComponent(string value, string propertyName)
    {
        RequireValue(value, propertyName);
        if (value.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0
            || value.Contains(Path.DirectorySeparatorChar)
            || value.Contains(Path.AltDirectorySeparatorChar))
        {
            throw new InvalidDataException($"{propertyName} must be a single valid name.");
        }
    }
}
