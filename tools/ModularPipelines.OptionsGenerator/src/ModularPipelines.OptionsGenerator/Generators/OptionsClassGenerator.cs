using System.Text;
using ModularPipelines.Attributes;
using ModularPipelines.OptionsGenerator.Models;

namespace ModularPipelines.OptionsGenerator.Generators;

/// <summary>
/// Generates strongly-typed C# options classes using the new CLI attribute system.
/// </summary>
public class OptionsClassGenerator : ICodeGenerator
{
    public Task<IReadOnlyList<GeneratedFile>> GenerateAsync(CliToolDefinition tool, CancellationToken cancellationToken = default)
    {
        tool = InheritedPropertyCollisionResolver.Resolve(tool);
        var files = new List<GeneratedFile>();

        foreach (var command in tool.Commands)
        {
            var content = GenerateOptionsClass(command, tool);
            var fileName = $"{command.ClassName}.Generated.cs";
            var relativePath = Path.Combine(tool.OutputDirectory, "Options", fileName);

            files.Add(new GeneratedFile
            {
                RelativePath = relativePath,
                Content = content
            });
        }

        return Task.FromResult<IReadOnlyList<GeneratedFile>>(files);
    }

    private static string GenerateOptionsClass(CliCommandDefinition command, CliToolDefinition tool)
    {
        var sb = new StringBuilder();

        // File header
        GenerateFileHeader(sb, command.DocumentationUrl);

        GenerateUsings(sb, command, tool);

        // Namespace
        sb.AppendLine($"namespace {tool.TargetNamespace}.Options;");
        sb.AppendLine();

        // XML documentation
        GeneratorUtils.GenerateXmlDocumentation(sb, command.Description, "");

        GenerateClassAttributes(sb, command);

        // Class declaration. The returned set contains the names emitted as
        // primary-constructor parameters, so a name scraped as both required and
        // optional can't produce two members (CS0102).
        var positionalArguments = CliPositionalArgument.MergeDuplicates(command.PositionalArguments);
        var existingPropertyNames = GenerateClassDeclaration(sb, command, positionalArguments);

        sb.AppendLine("{");
        GenerateProperties(sb, command, positionalArguments, existingPropertyNames);
        GenerateRequiredAlternativeValidation(sb, command, positionalArguments);
        sb.AppendLine("}");

        return sb.ToString();
    }

    private static void GenerateUsings(StringBuilder sb, CliCommandDefinition command, CliToolDefinition tool)
    {
        if (command.Options.Any(static option => option.IsSecret)
            || command.PositionalArguments.Any(static argument => argument.IsSecret))
        {
            sb.AppendLine("using ModularPipelines.Secrets;");
        }

        sb.AppendLine("using System.CodeDom.Compiler;");
        sb.AppendLine("using System.Diagnostics.CodeAnalysis;");
        sb.AppendLine("using ModularPipelines.Attributes;");

        // Include the existing Options namespace where the base class lives
        sb.AppendLine($"using {tool.TargetNamespace}.Options;");

        if (command.Options.Any(static option => option.RequiresModelsNamespace)
            || command.PositionalArguments.Any(static argument =>
                CliOptionDefinition.TypeRequiresModelsNamespace(argument.CSharpType)))
        {
            sb.AppendLine("using ModularPipelines.Models;");
        }

        if (command.Options.Any(o => o.ValidationConstraints is not null)
            || command.RequiredAlternativeGroups.Count > 0)
        {
            sb.AppendLine("using System.ComponentModel.DataAnnotations;");
        }

        // Include enums namespace if any options use enum types
        if (GeneratorUtils.UsesGeneratedEnums(tool, command))
        {
            sb.AppendLine($"using {tool.TargetNamespace}.Enums;");
        }

        sb.AppendLine();
    }

    private static void GenerateClassAttributes(StringBuilder sb, CliCommandDefinition command)
    {
        sb.AppendLine(GeneratorUtils.GeneratedCodeAttribute);
        sb.AppendLine("[ExcludeFromCodeCoverage]");
        // CliSubCommand attribute - contains only the subcommand parts (tool name comes from base class)
        if (command.CommandParts.Length > 0)
        {
            var args = string.Join(
                ", ",
                command.CommandParts.Select(GeneratorUtils.FormatStringLiteral));
            sb.AppendLine($"[CliSubCommand({args})]");
        }
    }

    private static void GenerateProperties(
        StringBuilder sb,
        CliCommandDefinition command,
        IReadOnlyList<CliPositionalArgument> positionalArguments,
        HashSet<string> existingPropertyNames)
    {
        // Properties for non-required options
        foreach (var option in command.Options.Where(o => !o.IsRequired))
        {
            if (!existingPropertyNames.Add(option.PropertyName))
            {
                continue; // Skip duplicates
            }
            GenerateProperty(sb, option);
            sb.AppendLine();
        }

        // Positional arguments - skip duplicates
        foreach (var positional in positionalArguments.Where(p => !p.IsRequired))
        {
            if (existingPropertyNames.Contains(positional.PropertyName))
            {
                continue; // Skip duplicates
            }
            GeneratePositionalArgument(sb, positional);
            existingPropertyNames.Add(positional.PropertyName);
            sb.AppendLine();
        }
    }

    private static void GenerateFileHeader(StringBuilder sb, string? documentationUrl)
    {
        GeneratorUtils.GenerateFileHeaderWithNullable(sb, documentationUrl);
    }

    /// <summary>
    /// Emits the class declaration and returns the member names emitted as
    /// primary-constructor parameters, so property emission can skip duplicates.
    /// </summary>
    private static HashSet<string> GenerateClassDeclaration(
        StringBuilder sb,
        CliCommandDefinition command,
        IReadOnlyList<CliPositionalArgument> positionalArguments)
    {
        var constructorParameters = GeneratorUtils.GetRequiredConstructorParameters(command, positionalArguments);
        var existingNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        if (constructorParameters.Count > 0)
        {
            // Use primary constructor for required parameters
            var parameters = new List<string>();

            foreach (var parameter in constructorParameters)
            {
                var attribute = parameter.Option is { } option
                    ? GeneratorUtils.GenerateCliAttributeString(option)
                    : GetPositionalAttributeString(parameter.PositionalArgument!);
                var secretAttribute = parameter.Option is { IsSecret: true } secretOption
                    ? $"{GeneratorUtils.GenerateSecretAttribute(secretOption)}, "
                    : parameter.IsSecret ? "SecretValue, " : "";
                parameters.Add(
                    $"    [property: {secretAttribute}{attribute}] " +
                    $"{parameter.CSharpType.TrimEnd('?')} {parameter.PropertyName}");
                existingNames.Add(parameter.PropertyName);
            }

            sb.AppendLine($"public record {command.ClassName}(");
            sb.AppendLine(string.Join($",{Environment.NewLine}", parameters));
            sb.AppendLine($") : {GetBaseTypes(command)}");
        }
        else
        {
            sb.AppendLine($"public record {command.ClassName} : {GetBaseTypes(command)}");
        }

        return existingNames;
    }

    private static string GetBaseTypes(CliCommandDefinition command) =>
        command.RequiredAlternativeGroups.Count > 0
            ? $"{command.ParentClassName}, IValidatableObject"
            : command.ParentClassName;

    private static void GenerateRequiredAlternativeValidation(
        StringBuilder sb,
        CliCommandDefinition command,
        IReadOnlyList<CliPositionalArgument> positionalArguments)
    {
        if (command.RequiredAlternativeGroups.Count == 0)
        {
            return;
        }

        sb.AppendLine("    /// <inheritdoc />");
        sb.AppendLine("    IEnumerable<ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)");
        sb.AppendLine("    {");
        foreach (var group in command.RequiredAlternativeGroups)
        {
            var propertyNames = group.PropertyNames.Distinct(StringComparer.Ordinal).ToArray();
            if (propertyNames.Length == 0)
            {
                throw new InvalidOperationException(
                    $"Required alternative group for {command.FullCommand} has no properties.");
            }

            var presenceExpression = string.Join(
                " || ",
                propertyNames.Select(propertyName => GetPresenceExpression(
                    command,
                    positionalArguments,
                    propertyName)));
            var memberNames = string.Join(", ", propertyNames.Select(propertyName => $"nameof({propertyName})"));
            var message = $"At least one of {FormatChoice(propertyNames)} must be specified.";

            sb.AppendLine($"        if (!({presenceExpression}))");
            sb.AppendLine("        {");
            sb.AppendLine($"            yield return new ValidationResult({GeneratorUtils.FormatStringLiteral(message)}, [{memberNames}]);");
            sb.AppendLine("        }");
        }
        sb.AppendLine("    }");
        sb.AppendLine();
    }

    private static string GetPresenceExpression(
        CliCommandDefinition command,
        IReadOnlyList<CliPositionalArgument> positionalArguments,
        string propertyName)
    {
        var option = command.Options.FirstOrDefault(candidate => candidate.PropertyName == propertyName);
        var csharpType = option?.PropertyType
                         ?? positionalArguments.FirstOrDefault(candidate => candidate.PropertyName == propertyName)
                             ?.CSharpType
                         ?? throw new InvalidOperationException(
                             $"Required alternative property {propertyName} was not generated for {command.FullCommand}.");

        if (option?.IsFlag == true)
        {
            return $"{propertyName} == true";
        }

        if (csharpType.TrimEnd('?').Equals("string", StringComparison.Ordinal))
        {
            return $"!string.IsNullOrWhiteSpace({propertyName})";
        }

        return CliOptionDefinition.TryGetCollectionShape(csharpType, out var isCollection) && isCollection
            ? $"{propertyName}?.Any() == true"
            : $"{propertyName} is not null";
    }

    private static string FormatChoice(IReadOnlyList<string> propertyNames) =>
        propertyNames.Count switch
        {
            0 => "a required value",
            1 => propertyNames[0],
            2 => $"{propertyNames[0]} or {propertyNames[1]}",
            _ => $"{string.Join(", ", propertyNames.Take(propertyNames.Count - 1))}, or {propertyNames[^1]}",
        };

    private static void GenerateProperty(StringBuilder sb, CliOptionDefinition option)
    {
        // XML documentation
        GeneratorUtils.GenerateXmlDocumentation(sb, option.Description);

        // Validation attributes
        if (option.ValidationConstraints is not null)
        {
            GeneratorUtils.GenerateValidationAttributes(
                sb,
                option.ValidationConstraints,
                useCliOptionValueAttributes: option.ValueArity == CliOptionValueArity.Optional);
        }

        // Secret attribute for sensitive values
        if (option.IsSecret)
        {
            sb.AppendLine($"    [{GeneratorUtils.GenerateSecretAttribute(option)}]");
        }

        // Command attribute
        var attribute = GeneratorUtils.GenerateCliAttributeString(option);
        sb.AppendLine($"    [{attribute}]");

        // Property
        sb.AppendLine($"    public {GetNewModifier(option.PropertyName)}{option.PropertyType} {option.PropertyName} {{ get; set; }}");
    }

    private static void GeneratePositionalArgument(StringBuilder sb, CliPositionalArgument positional)
    {
        GeneratorUtils.GenerateXmlDocumentation(sb, positional.Description);

        if (positional.IsSecret)
        {
            sb.AppendLine("    [SecretValue]");
        }

        var attrString = GetPositionalAttributeString(positional);
        sb.AppendLine($"    [{attrString}]");
        sb.AppendLine($"    public {positional.CSharpType} {positional.PropertyName} {{ get; set; }}");
    }

    private static string GetNewModifier(string propertyName) =>
        InheritedPropertyCollisionResolver.IsInheritedPropertyName(propertyName) ? "new " : "";

    private static string GetPositionalAttributeString(CliPositionalArgument positional)
    {
        var parts = new List<string> { positional.PositionIndex.ToString() };
        parts.Add($"Phase = CommandLinePhase.{positional.Phase}");

        if (positional.PrependOptionTerminator)
        {
            parts.Add("PrependOptionTerminator = true");
        }

        if (positional.RepeatOptionTerminator)
        {
            parts.Add("RepeatOptionTerminator = true");
        }

        if (positional.PrependOptionTerminatorIfValueStartsWithDash)
        {
            parts.Add("PrependOptionTerminatorIfValueStartsWithDash = true");
        }

        if (positional.IsValidationRequired ?? positional.IsRequired)
        {
            parts.Add("Required = true");
        }

        return $"CliArgument({string.Join(", ", parts)})";
    }
}
