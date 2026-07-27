using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.CodeAnalysis.CSharp;
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
        RequireIdentifier(tool.NamespacePrefix, "tool.namespacePrefix");
        RequireNamespace(tool.TargetNamespace, "tool.targetNamespace");
        RequireValue(tool.OutputDirectory, "tool.outputDirectory");

        if (tool.Commands is not { Count: > 0 })
        {
            throw new InvalidDataException("External tool metadata must define at least one command.");
        }

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
            ValidateCommand(command, tool.ToolName, tool.NamespacePrefix);
        }

        ValidateOptions(tool.GlobalOptions, "tool.globalOptions");
        ValidateOptions(tool.SupplementalGlobalOptions, "tool.supplementalGlobalOptions");
    }

    internal static string ValidateRelativeOutputPath(
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

        RejectLinkedPathComponents(root, candidate, propertyName);
        return candidate;
    }

    private static void ValidateCommand(
        CliCommandDefinition command,
        string toolName,
        string namespacePrefix)
    {
        RequireValue(command.FullCommand, "tool.commands[].fullCommand");
        ValidateCommandPath(command, toolName);
        RequireIdentifier(command.ClassName, "tool.commands[].className");
        RequireIdentifier(command.ParentClassName, "tool.commands[].parentClassName");
        RequireIdentifier(command.ToolNamespacePrefix, "tool.commands[].toolNamespacePrefix");
        if (!string.Equals(command.ToolNamespacePrefix, namespacePrefix, StringComparison.Ordinal))
        {
            throw new InvalidDataException(
                "tool.commands[].toolNamespacePrefix must match tool.namespacePrefix.");
        }

        ValidateOptionalIdentifier(command.SubDomainGroup, "tool.commands[].subDomainGroup");
        ValidateOptionalIdentifier(
            command.CommandGroupIdentifierOverride,
            "tool.commands[].commandGroupIdentifierOverride");
        ValidateOptions(command.Options, "tool.commands[].options");
        ValidatePositionalArguments(command.PositionalArguments);
        ValidateEnums(command.Enums, "tool.commands[].enums");
        ValidateCompatibilityMetadata(command);
        command.ValidateOperandCoverage();
    }

    private static void ValidateCommandPath(CliCommandDefinition command, string toolName)
    {
        foreach (var commandPart in command.CommandParts)
        {
            RequireValue(commandPart, "tool.commands[].commandParts[]");
            if (commandPart.Any(char.IsWhiteSpace))
            {
                throw new InvalidDataException(
                    "tool.commands[].commandParts[] must contain one command path component.");
            }
        }

        var normalizedFullCommand = string.Join(
            ' ',
            command.FullCommand.Split((char[]?) null, StringSplitOptions.RemoveEmptyEntries));
        var expectedFullCommand = string.Join(' ', new[] { toolName }.Concat(command.CommandParts));
        if (!string.Equals(
                normalizedFullCommand,
                expectedFullCommand,
                StringComparison.Ordinal))
        {
            throw new InvalidDataException(
                "tool.commands[].fullCommand must match tool.toolName plus "
                + "tool.commands[].commandParts.");
        }
    }

    private static void ValidateOptions(
        IReadOnlyList<CliOptionDefinition> options,
        string propertyName)
    {
        foreach (var option in options)
        {
            RequireIdentifier(option.PropertyName, $"{propertyName}[].propertyName");
            RequireTypeName(option.CSharpType, $"{propertyName}[].cSharpType");
            if (option.EnumDefinition is not null)
            {
                ValidateEnum(option.EnumDefinition, $"{propertyName}[].enumDefinition");
            }
        }
    }

    private static void ValidatePositionalArguments(
        IReadOnlyList<CliPositionalArgument> positionalArguments)
    {
        foreach (var positionalArgument in positionalArguments)
        {
            RequireIdentifier(
                positionalArgument.PropertyName,
                "tool.commands[].positionalArguments[].propertyName");
            RequireTypeName(
                positionalArgument.CSharpType,
                "tool.commands[].positionalArguments[].cSharpType");
        }
    }

    private static void ValidateEnums(
        IReadOnlyList<CliEnumDefinition> enums,
        string propertyName)
    {
        foreach (var enumDefinition in enums)
        {
            ValidateEnum(enumDefinition, $"{propertyName}[]");
        }
    }

    private static void ValidateEnum(CliEnumDefinition enumDefinition, string propertyName)
    {
        RequireIdentifier(enumDefinition.EnumName, $"{propertyName}.enumName");
        foreach (var value in enumDefinition.Values)
        {
            RequireIdentifier(value.MemberName, $"{propertyName}.values[].memberName");
        }
    }

    private static void ValidateCompatibilityMetadata(CliCommandDefinition command)
    {
        foreach (var property in command.CompatibilityProperties)
        {
            RequireIdentifier(
                property.PropertyName,
                "tool.commands[].compatibilityProperties[].propertyName");
            RequireTypeName(
                property.CSharpType,
                "tool.commands[].compatibilityProperties[].cSharpType");
            ValidateOptionalIdentifier(
                property.ForwardToPropertyName,
                "tool.commands[].compatibilityProperties[].forwardToPropertyName");
        }

        foreach (var method in command.CompatibilityMethods)
        {
            RequireIdentifier(
                method.MethodName,
                "tool.commands[].compatibilityMethods[].methodName");
        }
    }

    private static void RequireNamespace(string value, string propertyName)
    {
        RequireValue(value, propertyName);
        foreach (var component in value.Split('.'))
        {
            RequireIdentifier(component, propertyName);
        }
    }

    private static void ValidateOptionalIdentifier(string? value, string propertyName)
    {
        if (!string.IsNullOrWhiteSpace(value))
        {
            RequireIdentifier(value, propertyName);
        }
    }

    private static void RequireIdentifier(string value, string propertyName)
    {
        RequireValue(value, propertyName);
        if (!SyntaxFacts.IsValidIdentifier(value)
            || SyntaxFacts.GetKeywordKind(value) != SyntaxKind.None)
        {
            throw new InvalidDataException($"{propertyName} must be a valid C# identifier.");
        }
    }

    private static void RequireTypeName(string value, string propertyName)
    {
        RequireValue(value, propertyName);
        var type = SyntaxFactory.ParseTypeName(value);
        if (type.ContainsDiagnostics || type.ToFullString() != value)
        {
            throw new InvalidDataException($"{propertyName} must be a valid C# type name.");
        }
    }

    private static void RequireValue(string value, string propertyName)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new InvalidDataException($"{propertyName} is required.");
        }
    }

    private static void RejectLinkedPathComponents(
        string root,
        string candidate,
        string propertyName)
    {
        var relativePath = Path.GetRelativePath(root, candidate);
        var current = root;
        foreach (var component in relativePath.Split(
                     [Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar],
                     StringSplitOptions.RemoveEmptyEntries))
        {
            current = Path.Combine(current, component);
            var fileSystemInfo = new DirectoryInfo(current);
            if (!fileSystemInfo.Exists && fileSystemInfo.LinkTarget is null)
            {
                continue;
            }

            if ((fileSystemInfo.Attributes & FileAttributes.ReparsePoint) != 0
                || fileSystemInfo.LinkTarget is not null)
            {
                throw new InvalidDataException(
                    $"{propertyName} cannot traverse symbolic links or reparse points.");
            }
        }
    }
}
