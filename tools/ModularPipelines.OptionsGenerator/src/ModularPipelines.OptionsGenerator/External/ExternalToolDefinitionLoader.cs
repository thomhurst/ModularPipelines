using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.CodeAnalysis.CSharp;
using ModularPipelines.Attributes;
using ModularPipelines.Options;
using ModularPipelines.OptionsGenerator.Generators;
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

        RequireValue(tool.OwnershipId ?? string.Empty, "tool.ownershipId");
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

        ValidateOptions(tool.GlobalOptions, "tool.globalOptions", allowRequired: false);
        ValidateOptions(
            tool.SupplementalGlobalOptions,
            "tool.supplementalGlobalOptions",
            allowRequired: false);
        ValidateEquivalentEnumDefinitions(tool);

        var globalOptions = tool.GetGlobalOptions();
        foreach (var command in tool.Commands)
        {
            ValidateUniqueGeneratedMemberNames(command, globalOptions);
            ValidateCompatibilityMetadata(command, globalOptions);
            ValidateUniqueEffectiveSwitches(command, globalOptions);
        }
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
        var expectedParentClassName = $"{namespacePrefix}Options";
        if (!string.Equals(
                command.ParentClassName,
                expectedParentClassName,
                StringComparison.Ordinal))
        {
            throw new InvalidDataException(
                $"tool.commands[].parentClassName must be '{expectedParentClassName}'.");
        }

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

        RequireIdentifier(
            GeneratorUtils.GenerateMethodNameFromCommandParts(command.CommandParts),
            "tool.commands[].commandParts derived method name");
        if (!string.IsNullOrWhiteSpace(command.SubDomainGroup))
        {
            RequireIdentifier(
                GeneratorUtils.GenerateMethodNameFromLastCommandPart(command),
                "tool.commands[].commandParts derived sub-domain method name");
        }
    }

    private static void ValidateOptions(
        IReadOnlyList<CliOptionDefinition> options,
        string propertyName,
        bool allowRequired = true)
    {
        foreach (var option in options)
        {
            ValidateOption(option, propertyName);
            if (!allowRequired && option.IsRequired)
            {
                throw new InvalidDataException(
                    $"{propertyName}[].isRequired is not supported for global options.");
            }
        }
    }

    private static void ValidateOption(CliOptionDefinition option, string propertyName)
    {
        RequireSingleToken(option.SwitchName, $"{propertyName}[].switchName");

        if (option.ShortForm is not null)
        {
            RequireSingleToken(option.ShortForm, $"{propertyName}[].shortForm");
        }

        RequireIdentifier(option.PropertyName, $"{propertyName}[].propertyName");
        RequireTypeName(option.CSharpType, $"{propertyName}[].cSharpType");
        ValidateFlagType(option, propertyName);
        ValidateOptionalCollectionShape(option, propertyName);
        ValidateSecretValueKeys(option, propertyName);

        if (option.EnumDefinition is not null)
        {
            ValidateEnum(option.EnumDefinition, $"{propertyName}[].enumDefinition");
        }
    }

    private static void ValidateOptionalCollectionShape(
        CliOptionDefinition option,
        string propertyName)
    {
        if (option.ValueArity != CliOptionValueArity.Optional
            || option.AcceptsMultipleValues
            || option.GroupValues)
        {
            return;
        }

        if (CliOptionDefinition.TryGetCollectionShape(option.CSharpType, out var isCollection))
        {
            if (option.IsCollection is not null && option.IsCollection != isCollection)
            {
                throw new InvalidDataException(
                    $"{propertyName}[].isCollection conflicts with the resolved cSharpType.");
            }

            return;
        }

        if (option.IsCollection is null)
        {
            throw new InvalidDataException(
                $"{propertyName}[].isCollection must be true or false when an optional cSharpType "
                + "is unavailable to the options generator.");
        }
    }

    private static void ValidateFlagType(CliOptionDefinition option, string propertyName)
    {
        if (option.IsFlag && option.ValueArity == CliOptionValueArity.Optional)
        {
            throw new InvalidDataException(
                $"{propertyName}[].valueArity cannot be optional when isFlag is true.");
        }

        if (option.IsFlag && !IsSupportedFlagType(option.CSharpType))
        {
            throw new InvalidDataException(
                $"{propertyName}[].cSharpType must be bool, bool?, int, or int? when isFlag is true.");
        }
    }

    private static void ValidateSecretValueKeys(
        CliOptionDefinition option,
        string propertyName)
    {
        if (option.SecretValueKeys.Count == 0)
        {
            return;
        }

        if (!option.IsSecret)
        {
            throw new InvalidDataException(
                $"{propertyName}[].isSecret must be true when secretValueKeys are declared.");
        }

        if (option.ValueArity == CliOptionValueArity.Optional)
        {
            throw new InvalidDataException(
                $"{propertyName}[].secretValueKeys do not support optional option values.");
        }

        if (!option.IsKeyValue || !IsReadOnlyListKeyValueType(option.CSharpType))
        {
            throw new InvalidDataException(
                $"{propertyName}[].secretValueKeys require isKeyValue=true and cSharpType IReadOnlyList<KeyValue>.");
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

        var duplicateMember = enumDefinition.Values
            .GroupBy(value => value.MemberName, StringComparer.Ordinal)
            .FirstOrDefault(group => group.Skip(1).Any());
        if (duplicateMember is not null)
        {
            throw new InvalidDataException(
                $"{propertyName} contains duplicate member name '{duplicateMember.Key}'.");
        }
    }

    private static bool IsReadOnlyListKeyValueType(string cSharpType)
    {
        var type = cSharpType.Replace(" ", string.Empty, StringComparison.Ordinal);
        return type is "IReadOnlyList<KeyValue>"
            or "IReadOnlyList<KeyValue>?"
            or "System.Collections.Generic.IReadOnlyList<ModularPipelines.Models.KeyValue>"
            or "System.Collections.Generic.IReadOnlyList<ModularPipelines.Models.KeyValue>?";
    }

    private static bool IsSupportedFlagType(string cSharpType)
    {
        var type = cSharpType
            .Replace(" ", string.Empty, StringComparison.Ordinal)
            .Replace("global::", string.Empty, StringComparison.Ordinal);
        return type is "bool"
            or "bool?"
            or "int"
            or "int?"
            or "System.Boolean"
            or "System.Boolean?"
            or "System.Int32"
            or "System.Int32?";
    }

    private static void ValidateEquivalentEnumDefinitions(CliToolDefinition tool)
    {
        var definitions = tool.Commands
            .SelectMany(command => command.Enums.Concat(
                command.Options
                    .Where(option => option.EnumDefinition is not null)
                    .Select(option => option.EnumDefinition!)))
            .Concat(tool.GlobalOptions
                .Concat(tool.SupplementalGlobalOptions)
                .Where(option => option.EnumDefinition is not null)
                .Select(option => option.EnumDefinition!));

        foreach (var group in definitions.GroupBy(
                     definition => definition.EnumName,
                     StringComparer.Ordinal))
        {
            var expected = group.First();
            if (group.Skip(1).Any(definition => !AreEquivalent(expected, definition)))
            {
                throw new InvalidDataException(
                    $"External tool metadata contains conflicting definitions for enum '{group.Key}'.");
            }
        }
    }

    private static bool AreEquivalent(
        CliEnumDefinition first,
        CliEnumDefinition second) =>
        string.Equals(first.Description, second.Description, StringComparison.Ordinal)
        && first.Values.Count == second.Values.Count
        && first.Values.Zip(second.Values).All(pair =>
            string.Equals(pair.First.MemberName, pair.Second.MemberName, StringComparison.Ordinal)
            && string.Equals(pair.First.CliValue, pair.Second.CliValue, StringComparison.Ordinal)
            && string.Equals(pair.First.Description, pair.Second.Description, StringComparison.Ordinal)
            && pair.First.NumericValue == pair.Second.NumericValue);

    private static void ValidateUniqueEffectiveSwitches(
        CliCommandDefinition command,
        IReadOnlyList<CliOptionDefinition> globalOptions)
    {
        var propertiesBySwitch = new Dictionary<string, string>(StringComparer.Ordinal);
        foreach (var option in globalOptions.Concat(command.Options))
        {
            RegisterSwitch(option.SwitchName, option.PropertyName, propertiesBySwitch, command);
            if (!string.IsNullOrWhiteSpace(option.ShortForm)
                && !string.Equals(option.SwitchName, option.ShortForm, StringComparison.Ordinal))
            {
                RegisterSwitch(option.ShortForm, option.PropertyName, propertiesBySwitch, command);
            }
        }
    }

    private static void RegisterSwitch(
        string switchName,
        string propertyName,
        IDictionary<string, string> propertiesBySwitch,
        CliCommandDefinition command)
    {
        if (propertiesBySwitch.TryGetValue(switchName, out var existingProperty))
        {
            throw new InvalidDataException(
                $"Command '{command.FullCommand}' defines CLI switch '{switchName}' more than once "
                + $"on properties '{existingProperty}' and '{propertyName}'.");
        }

        propertiesBySwitch.Add(switchName, propertyName);
    }

    internal static void ValidateCompatibilityMetadata(
        CliCommandDefinition command,
        IReadOnlyList<CliOptionDefinition> globalOptions)
    {
        var (forwardingTargets, writableForwardingTargets, initOnlyForwardingTargets) =
            GetCompatibilityForwardingTargets(command, globalOptions);

        foreach (var property in command.CompatibilityProperties)
        {
            ValidateCompatibilityProperty(
                property,
                command,
                forwardingTargets,
                writableForwardingTargets,
                initOnlyForwardingTargets);
        }

        foreach (var method in command.CompatibilityMethods)
        {
            RequireIdentifier(
                method.MethodName,
                "tool.commands[].compatibilityMethods[].methodName");
        }
    }

    private static (
        IReadOnlySet<string> All,
        IReadOnlyDictionary<string, string> Writable,
        IReadOnlyDictionary<string, string> InitOnly) GetCompatibilityForwardingTargets(
            CliCommandDefinition command,
            IReadOnlyList<CliOptionDefinition> globalOptions)
    {
        var writableTargets = command.Options
            .Where(option => !option.IsRequired)
            .Select(option => (option.PropertyName, CSharpType: option.PropertyType))
            .Concat(command.PositionalArguments
                .Where(argument => !argument.IsRequired)
                .Select(argument => (argument.PropertyName, argument.CSharpType)))
            .Concat(globalOptions.Select(option => (option.PropertyName, CSharpType: option.PropertyType)));
        var initOnlyTargets = command.Options
            .Where(option => option.IsRequired)
            .Select(option => (option.PropertyName, CSharpType: option.PropertyType))
            .Concat(command.PositionalArguments
                .Where(argument => argument.IsRequired)
                .Select(argument => (argument.PropertyName, argument.CSharpType)));
        var writable = CreateCompatibilityTargetMap(command, writableTargets, "writable");
        var initOnly = CreateCompatibilityTargetMap(command, initOnlyTargets, "init-only");

        var all = writable.Keys
            .Concat(command.Options.Select(option => option.PropertyName))
            .Concat(command.PositionalArguments.Select(argument => argument.PropertyName))
            .Concat(typeof(CommandLineToolOptions)
                .GetProperties()
                .Select(property => property.Name))
            .ToHashSet(StringComparer.Ordinal);

        return (all, writable, initOnly);
    }

    private static IReadOnlyDictionary<string, string> CreateCompatibilityTargetMap(
        CliCommandDefinition command,
        IEnumerable<(string PropertyName, string CSharpType)> targets,
        string targetKind)
    {
        var result = new Dictionary<string, string>(StringComparer.Ordinal);
        foreach (var target in targets)
        {
            // One generated property may represent the same operand from multiple
            // metadata sources. Coalesce it only when every source agrees on its type.
            if (result.TryGetValue(target.PropertyName, out var existingType)
                && !existingType.Equals(target.CSharpType, StringComparison.Ordinal))
            {
                throw new InvalidDataException(
                    $"Command '{command.FullCommand}' defines {targetKind} compatibility target "
                    + $"'{target.PropertyName}' with conflicting types '{existingType}' and '{target.CSharpType}'.");
            }

            result[target.PropertyName] = target.CSharpType;
        }

        return result;
    }

    private static void ValidateCompatibilityProperty(
        CliCompatibilityProperty property,
        CliCommandDefinition command,
        IReadOnlySet<string> forwardingTargets,
        IReadOnlyDictionary<string, string> writableForwardingTargets,
        IReadOnlyDictionary<string, string> initOnlyForwardingTargets)
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
        if (property.ForwardToPropertyName is null)
        {
            return;
        }

        if (!forwardingTargets.Contains(property.ForwardToPropertyName))
        {
            throw new InvalidDataException(
                $"Compatibility property '{property.PropertyName}' on command "
                + $"'{command.FullCommand}' forwards to missing property "
                + $"'{property.ForwardToPropertyName}'.");
        }

        var forwardingTargetType = GetCompatibilityForwardingTargetType(
            property,
            command,
            writableForwardingTargets,
            initOnlyForwardingTargets);
        ValidateCompatibilityForwardingTypes(property, command, forwardingTargetType);
    }

    private static string GetCompatibilityForwardingTargetType(
        CliCompatibilityProperty property,
        CliCommandDefinition command,
        IReadOnlyDictionary<string, string> writableForwardingTargets,
        IReadOnlyDictionary<string, string> initOnlyForwardingTargets)
    {
        if (writableForwardingTargets.TryGetValue(property.ForwardToPropertyName!, out var targetType)
            || (property.UseInitAccessor
                && initOnlyForwardingTargets.TryGetValue(property.ForwardToPropertyName!, out targetType)))
        {
            return targetType;
        }

        throw new InvalidDataException(
            $"Compatibility property '{property.PropertyName}' on command "
            + $"'{command.FullCommand}' forwards to init-only property "
            + $"'{property.ForwardToPropertyName}'.");
    }

    private static void ValidateCompatibilityForwardingTypes(
        CliCompatibilityProperty property,
        CliCommandDefinition command,
        string forwardingTargetType)
    {
        var propertyType = SyntaxFactory.ParseTypeName(property.CSharpType);
        var targetType = SyntaxFactory.ParseTypeName(forwardingTargetType);
        var typesAreCompatible = property.ForwardingKind switch
        {
            CliCompatibilityForwardingKind.Direct => propertyType.IsEquivalentTo(targetType),
            CliCompatibilityForwardingKind.ScalarToCollection =>
                propertyType.IsEquivalentTo(SyntaxFactory.ParseTypeName("string?"))
                && targetType.IsEquivalentTo(SyntaxFactory.ParseTypeName("IEnumerable<string>?")),
            CliCompatibilityForwardingKind.NullableInt32ToString =>
                propertyType.IsEquivalentTo(SyntaxFactory.ParseTypeName("int?"))
                && targetType.IsEquivalentTo(SyntaxFactory.ParseTypeName("string?")),
            CliCompatibilityForwardingKind.NullableStringToRequiredString =>
                propertyType.IsEquivalentTo(SyntaxFactory.ParseTypeName("string?"))
                && targetType.IsEquivalentTo(SyntaxFactory.ParseTypeName("string")),
            CliCompatibilityForwardingKind.NullableInt32ToRequiredString =>
                propertyType.IsEquivalentTo(SyntaxFactory.ParseTypeName("int?"))
                && targetType.IsEquivalentTo(SyntaxFactory.ParseTypeName("string")),
            CliCompatibilityForwardingKind.NullableInt32ToStringCollection =>
                propertyType.IsEquivalentTo(SyntaxFactory.ParseTypeName("int?"))
                && targetType.IsEquivalentTo(SyntaxFactory.ParseTypeName("IEnumerable<string>?")),
            _ => false,
        };
        if (!typesAreCompatible)
        {
            throw new InvalidDataException(
                $"Compatibility property '{property.PropertyName}' on command "
                + $"'{command.FullCommand}' cannot use {property.ForwardingKind} forwarding from type '{property.CSharpType}' "
                + $"to '{forwardingTargetType}' property '{property.ForwardToPropertyName}'.");
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
        if (type.ContainsDiagnostics
            || type.ToFullString() != value
            || type.DescendantTokens().Any(token =>
                token.RawKind == (int) SyntaxKind.VoidKeyword
                || token.ValueText.Equals("var", StringComparison.Ordinal)))
        {
            throw new InvalidDataException(
                $"{propertyName} must be a valid C# property or parameter type.");
        }
    }

    private static void RequireValue(string value, string propertyName)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new InvalidDataException($"{propertyName} is required.");
        }
    }

    private static void RequireSingleToken(string value, string propertyName)
    {
        RequireValue(value, propertyName);
        if (value.Any(char.IsWhiteSpace))
        {
            throw new InvalidDataException($"{propertyName} must contain one CLI token.");
        }
    }

    private static void RejectLinkedPathComponents(
        string root,
        string candidate,
        string propertyName)
    {
        RejectLinkedPath(root, propertyName);

        var relativePath = Path.GetRelativePath(root, candidate);
        var current = root;
        foreach (var component in relativePath.Split(
                     [Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar],
                     StringSplitOptions.RemoveEmptyEntries))
        {
            current = Path.Combine(current, component);
            RejectLinkedPath(current, propertyName);
        }
    }

    private static void ValidateUniqueGeneratedMemberNames(
        CliCommandDefinition command,
        IReadOnlyList<CliOptionDefinition> globalOptions)
    {
        var duplicate = globalOptions
            .Select(option => option.PropertyName)
            .Concat(command.Options.Select(option => option.PropertyName))
            .Concat(command.PositionalArguments.Select(argument => argument.PropertyName))
            .GroupBy(name => name, StringComparer.OrdinalIgnoreCase)
            .FirstOrDefault(group => group.Skip(1).Any());
        if (duplicate is not null)
        {
            throw new InvalidDataException(
                $"tool.commands[] contains duplicate generated member name '{duplicate.Key}'.");
        }
    }

    private static void RejectLinkedPath(string path, string propertyName)
    {
        var fileSystemInfo = new DirectoryInfo(path);
        if (!fileSystemInfo.Exists && fileSystemInfo.LinkTarget is null)
        {
            return;
        }

        if ((fileSystemInfo.Attributes & FileAttributes.ReparsePoint) != 0
            || fileSystemInfo.LinkTarget is not null)
        {
            throw new InvalidDataException(
                $"{propertyName} cannot traverse symbolic links or reparse points.");
        }
    }
}
