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

        var positionalArguments = CliPositionalArgument.MergeDuplicates(command.PositionalArguments);
        var constructorParameters = GeneratorUtils.GetRequiredConstructorParameters(command, positionalArguments);

        var supportsAlternateInputModes = SupportsAlternateInputModes(command, positionalArguments);
        var requiresValueValidation = constructorParameters.Any(parameter =>
            IsCollectionParameter(parameter)
            || (RequiresConstructorValue(parameter)
                && CliOptionDefinition.MayBeReferenceType(parameter.CSharpType)));
        var usesExplicitRequiredConstructor = supportsAlternateInputModes || requiresValueValidation
            || command.RequiredOptions.Any(static option => option.IsFlag);

        // Parameter tags belong on the primary declaration or the explicit constructor.
        GeneratorUtils.GenerateConstructorXmlDocumentation(
            sb, command, constructorParameters, includeParameters: !usesExplicitRequiredConstructor);
        GenerateClassAttributes(sb, command);

        // Class declaration. The returned set contains the names emitted as
        // primary-constructor parameters, so a name scraped as both required and
        // optional can't produce two members (CS0102).
        var existingPropertyNames = GenerateClassDeclaration(
            sb,
            command,
            positionalArguments,
            usePrimaryConstructor: !usesExplicitRequiredConstructor);

        sb.AppendLine("{");
        if (usesExplicitRequiredConstructor)
        {
            GenerateRequiredConstructor(
                sb,
                command,
                positionalArguments,
                includePrivateParameterlessConstructor: supportsAlternateInputModes);
            if (!supportsAlternateInputModes)
            {
                // Alternate-input factories leave operation values unset, so they cannot
                // promise the non-null outputs of positional record deconstruction.
                GenerateRequiredDeconstruct(sb, command, positionalArguments);
            }
        }

        if (supportsAlternateInputModes)
        {
            GenerateAlternateInputFactories(sb, command);
        }

        GenerateProperties(
            sb,
            command,
            positionalArguments,
            existingPropertyNames,
            usesExplicitRequiredConstructor,
            requiredPropertiesAreNonNullable: usesExplicitRequiredConstructor && !supportsAlternateInputModes);
        GenerateValidation(sb, command, positionalArguments);
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
            || RequiresCommandValidation(command))
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
        HashSet<string> existingPropertyNames,
        bool includeRequiredProperties,
        bool requiredPropertiesAreNonNullable)
    {
        // Required definitions own colliding names, including with explicit constructors.
        foreach (var option in command.Options
                     .Where(option => includeRequiredProperties || !option.IsRequired)
                     .OrderByDescending(static option => option.IsRequired))
        {
            if (!existingPropertyNames.Add(option.PropertyName))
            {
                continue; // Skip duplicates
            }
            GenerateProperty(sb, option, requiredPropertiesAreNonNullable);
            sb.AppendLine();
        }

        // Positional arguments - skip duplicates
        foreach (var positional in positionalArguments.Where(positional =>
                     includeRequiredProperties || !positional.IsRequired))
        {
            if (existingPropertyNames.Contains(positional.PropertyName))
            {
                continue; // Skip duplicates
            }
            GeneratePositionalArgument(sb, positional, requiredPropertiesAreNonNullable);
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
        IReadOnlyList<CliPositionalArgument> positionalArguments,
        bool usePrimaryConstructor)
    {
        var constructorParameters = GeneratorUtils.GetRequiredConstructorParameters(command, positionalArguments);
        var existingNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        if (constructorParameters.Count > 0 && usePrimaryConstructor)
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
                    $"{GetConstructorParameterType(parameter)} {parameter.PropertyName}");
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
        RequiresCommandValidation(command)
            ? $"{command.ParentClassName}, IValidatableObject"
            : command.ParentClassName;

    private static bool RequiresCommandValidation(CliCommandDefinition command) =>
        command.RequiredAlternativeGroups.Count > 0
        || SupportsAlternateInputModes(command, CliPositionalArgument.MergeDuplicates(command.PositionalArguments));

    private static bool SupportsAlternateInputModes(
        CliCommandDefinition command,
        IReadOnlyList<CliPositionalArgument> positionalArguments) =>
        command.RequiredOptions.Count > 0
        && positionalArguments.All(static positional => !positional.IsRequired)
        && (HasOption(command, "--cli-input-json")
            || HasOption(command, "--generate-cli-skeleton"));

    private static bool HasOption(CliCommandDefinition command, string switchName) =>
        command.Options.Any(option =>
            option.SwitchName.Equals(switchName, StringComparison.OrdinalIgnoreCase));

    private static void GenerateRequiredConstructor(
        StringBuilder sb,
        CliCommandDefinition command,
        IReadOnlyList<CliPositionalArgument> positionalArguments,
        bool includePrivateParameterlessConstructor)
    {
        var constructorParameters = GeneratorUtils.GetRequiredConstructorParameters(
            command,
            positionalArguments);
        var parameterDeclarations = constructorParameters.Select(parameter =>
            $"        {GetConstructorParameterType(parameter)} {parameter.PropertyName}");

        if (includePrivateParameterlessConstructor)
        {
            // Record copies retain the factory invariant when public input selectors change.
            sb.AppendLine("    private readonly bool _requiresAlternateInput;");
            sb.AppendLine();
        }

        GeneratorUtils.GenerateConstructorXmlDocumentation(sb, command, constructorParameters, indent: "    ");
        sb.AppendLine($"    public {command.ClassName}(");
        sb.AppendLine(string.Join($",{Environment.NewLine}", parameterDeclarations));
        sb.AppendLine("    )");
        sb.AppendLine("    {");
        foreach (var parameter in constructorParameters)
        {
            if (parameter.Option is { IsFlag: true, NegatedSwitchName: null })
            {
                sb.AppendLine($"        if (!{parameter.PropertyName})");
                sb.AppendLine("        {");
                sb.AppendLine("            throw new global::System.ArgumentException(");
                sb.AppendLine("                \"Required flag must be enabled to emit its switch.\",");
                sb.AppendLine($"                nameof({parameter.PropertyName}));");
                sb.AppendLine("        }");
            }
            else if (IsCollectionParameter(parameter))
            {
                GenerateCollectionSnapshot(sb, parameter);
            }
            else if (RequiresConstructorValue(parameter)
                     && CliOptionDefinition.MayBeReferenceType(parameter.CSharpType))
            {
                sb.AppendLine($"        global::System.ArgumentNullException.ThrowIfNull({parameter.PropertyName});");
            }

            sb.AppendLine($"        this.{parameter.PropertyName} = {parameter.PropertyName};");
        }

        sb.AppendLine("    }");
        sb.AppendLine();
        if (!includePrivateParameterlessConstructor)
        {
            return;
        }

        sb.AppendLine($"    private {command.ClassName}()");
        sb.AppendLine("    {");
        sb.AppendLine("        _requiresAlternateInput = true;");
        sb.AppendLine("    }");
        sb.AppendLine();
    }

    private static void GenerateCollectionSnapshot(
        StringBuilder sb,
        GeneratorUtils.RequiredConstructorParameter parameter)
    {
        var required = RequiresConstructorValue(parameter);
        var snapshot = CliOptionDefinition.GetCollectionSnapshotExpression(
            parameter.CSharpType.TrimEnd('?'), parameter.PropertyName);
        if (!required)
        {
            sb.AppendLine($"        if ({parameter.PropertyName} is not null)");
        }

        sb.AppendLine("        {");
        if (required)
        {
            sb.AppendLine($"            global::System.ArgumentNullException.ThrowIfNull({parameter.PropertyName});");
        }

        sb.AppendLine($"            var materialized = {snapshot};");
        if (required)
        {
            sb.AppendLine("            if (!global::System.Linq.Enumerable.Any(global::System.Linq.Enumerable.Cast<object>(materialized), static value => value is not null))");
            sb.AppendLine("            {");
            sb.AppendLine("                throw new global::System.ArgumentException(");
            sb.AppendLine("                    \"Required collection must contain at least one value.\",");
            sb.AppendLine($"                    nameof({parameter.PropertyName}));");
            sb.AppendLine("            }");
        }

        sb.AppendLine();
        sb.AppendLine($"            {parameter.PropertyName} = materialized;");
        sb.AppendLine("        }");
    }

    private static void GenerateRequiredDeconstruct(
        StringBuilder sb,
        CliCommandDefinition command,
        IReadOnlyList<CliPositionalArgument> positionalArguments)
    {
        var constructorParameters = GeneratorUtils.GetRequiredConstructorParameters(
            command,
            positionalArguments);
        var parameters = constructorParameters.Select(parameter =>
            $"out {GetConstructorParameterType(parameter)} {parameter.PropertyName}");
        sb.AppendLine($"    public void Deconstruct({string.Join(", ", parameters)})");
        sb.AppendLine("    {");
        foreach (var parameter in constructorParameters)
        {
            var value = RequiresNullableFlagProperty(parameter.Option)
                ? $"this.{parameter.PropertyName}.GetValueOrDefault()"
                : $"this.{parameter.PropertyName}";
            sb.AppendLine($"        {parameter.PropertyName} = {value};");
        }

        sb.AppendLine("    }");
        sb.AppendLine();
    }

    private static bool RequiresConstructorValue(GeneratorUtils.RequiredConstructorParameter parameter) =>
        parameter.PositionalArgument?.IsValidationRequired != false;

    private static string GetConstructorParameterType(GeneratorUtils.RequiredConstructorParameter parameter) =>
        parameter.CSharpType.TrimEnd('?') + (RequiresConstructorValue(parameter) ? "" : "?");

    private static bool IsCollectionParameter(
        GeneratorUtils.RequiredConstructorParameter parameter) =>
        CliOptionDefinition.TryGetCollectionShape(parameter.CSharpType.TrimEnd('?'), out var isCollection)
            ? isCollection
            : parameter.Option?.IsCollection == true;

    private static bool RequiresNullableFlagProperty(CliOptionDefinition? option) =>
        option is { IsFlag: true, NegatedSwitchName: not null };

    private static void GenerateAlternateInputFactories(
        StringBuilder sb,
        CliCommandDefinition command)
    {
        if (HasOption(command, "--cli-input-json"))
        {
            sb.AppendLine($"    public static {command.ClassName} FromCliInputJson(string cliInputJson)");
            sb.AppendLine("    {");
            sb.AppendLine("        global::System.ArgumentException.ThrowIfNullOrWhiteSpace(cliInputJson);");
            sb.AppendLine("        return new() { CliInputJson = cliInputJson };");
            sb.AppendLine("    }");
            sb.AppendLine();
        }

        if (HasOption(command, "--generate-cli-skeleton"))
        {
            sb.AppendLine($"    public static {command.ClassName} ForCliSkeleton(string generateCliSkeleton = \"input\") =>");
            sb.AppendLine("        generateCliSkeleton is \"input\" or \"yaml-input\"");
            sb.AppendLine("            ? new() { GenerateCliSkeleton = generateCliSkeleton }");
            sb.AppendLine("            : throw new global::System.ArgumentOutOfRangeException(");
            sb.AppendLine("                nameof(generateCliSkeleton),");
            sb.AppendLine("                generateCliSkeleton,");
            sb.AppendLine("                \"Required operation values may only be omitted for input or yaml-input skeletons.\");");
            sb.AppendLine();
        }
    }

    private static void GenerateValidation(
        StringBuilder sb,
        CliCommandDefinition command,
        IReadOnlyList<CliPositionalArgument> positionalArguments)
    {
        var supportsAlternateInputModes = SupportsAlternateInputModes(command, positionalArguments);
        if (command.RequiredAlternativeGroups.Count == 0 && !supportsAlternateInputModes)
        {
            return;
        }

        sb.AppendLine("    /// <inheritdoc />");
        sb.AppendLine("    IEnumerable<ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)");
        sb.AppendLine("    {");
        if (supportsAlternateInputModes)
        {
            List<string> alternateInputs = [];
            if (HasOption(command, "--cli-input-json"))
            {
                alternateInputs.Add("!string.IsNullOrWhiteSpace(CliInputJson)");
            }

            if (HasOption(command, "--generate-cli-skeleton"))
            {
                alternateInputs.Add("GenerateCliSkeleton is \"input\" or \"yaml-input\"");
            }

            var alternateInputSelected = string.Join(" || ", alternateInputs);
            sb.AppendLine($"        if (_requiresAlternateInput && !({alternateInputSelected}))");
            sb.AppendLine("        {");
            sb.AppendLine("            yield return new ValidationResult(\"An alternate input must remain selected for an instance created without required operation values.\");");
            sb.AppendLine("            yield break;");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine($"        if ({alternateInputSelected})");
            sb.AppendLine("        {");
            sb.AppendLine("            yield break;");
            sb.AppendLine("        }");
            sb.AppendLine();
        }

        foreach (var group in command.RequiredAlternativeGroups)
        {
            GenerateGroupValidation(sb, command, positionalArguments, group, group.IsRequired, activation: null);
        }

        sb.AppendLine("        yield break;");
        sb.AppendLine("    }");
        sb.AppendLine();
    }

    private static void GenerateGroupValidation(
        StringBuilder sb,
        CliCommandDefinition command,
        IReadOnlyList<CliPositionalArgument> positionalArguments,
        CliRequiredAlternativeGroup group,
        bool required,
        string? activation)
    {
        var propertyNames = group.PropertyNames.Distinct(StringComparer.Ordinal).ToArray();
        if (propertyNames.Length == 0)
        {
            throw new InvalidOperationException($"Required alternative group for {command.FullCommand} has no properties.");
        }

        string Presence(string propertyName) => GetPresenceExpression(command, positionalArguments, propertyName);
        string GroupPresence(CliRequiredAlternativeGroup nested) =>
            $"({string.Join(" || ", nested.PropertyNames.Distinct(StringComparer.Ordinal).Select(Presence))})";

        var presence = GroupPresence(group);
        GenerateGroupPresenceValidation(sb, group, required, activation, propertyNames, presence, Presence, GroupPresence);

        var activeGroup = activation is null ? presence : $"{activation} && {presence}";
        if (!group.IsChoice)
        {
            foreach (var member in group.Members.Where(member => member.IsRequired))
            {
                WriteValidationFailure(sb, $"!({Presence(member.PropertyName)})", activeGroup,
                    $"{member.PropertyName} must be specified when other arguments in this group are specified.",
                    [member.PropertyName]);
            }
        }

        foreach (var nested in group.Groups)
        {
            // A choice activates only the selected branches. A bundle can require
            // one of its nested groups whenever any part of the bundle is supplied.
            GenerateGroupValidation(sb, command, positionalArguments, nested,
                required: !group.IsChoice && nested.IsRequired, activeGroup);
        }
    }

    private static void GenerateGroupPresenceValidation(
        StringBuilder sb,
        CliRequiredAlternativeGroup group,
        bool required,
        string? activation,
        string[] propertyNames,
        string presence,
        Func<string, string> getPresence,
        Func<CliRequiredAlternativeGroup, string> getGroupPresence)
    {
        if (group.IsChoice && group.IsMutuallyExclusive)
        {
            var branches = group.Members.Select(member => member.PropertyName).Distinct(StringComparer.Ordinal)
                .Select(getPresence).Concat(group.Groups.Select(getGroupPresence));
            var count = string.Join(" + ", branches.Select(expression => $"({expression} ? 1 : 0)"));
            var branchNames = group.Members.Select(member => member.PropertyName).Distinct(StringComparer.Ordinal)
                .Concat(group.Groups.Select(nested =>
                    $"({FormatChoice([.. nested.PropertyNames.Distinct(StringComparer.Ordinal)])})")).ToArray();
            var cardinality = required ? "Exactly one" : "At most one";
            WriteValidationFailure(sb, $"{count} {(required ? "!= 1" : "> 1")}", activation,
                $"{cardinality} of {FormatChoice(branchNames)} {(required ? "must" : "may")} be specified.", propertyNames);
        }
        else if (required)
        {
            WriteValidationFailure(sb, $"!{presence}", activation,
                $"At least one of {FormatChoice(propertyNames)} must be specified.", propertyNames);
        }
    }

    private static void WriteValidationFailure(
        StringBuilder sb, string invalidExpression, string? activation, string message, string[] propertyNames)
    {
        var condition = activation is null ? invalidExpression : $"{activation} && ({invalidExpression})";
        var memberNames = string.Join(", ", propertyNames.Select(propertyName => $"nameof({propertyName})"));
        sb.AppendLine($"        if ({condition})");
        sb.AppendLine("        {");
        sb.AppendLine($"            yield return new ValidationResult({GeneratorUtils.FormatStringLiteral(message)}, [{memberNames}]);");
        sb.AppendLine("        }");
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

    private static string FormatChoice(string[] propertyNames) =>
        propertyNames.Length switch
        {
            0 => "a required value",
            1 => propertyNames[0],
            2 => $"{propertyNames[0]} or {propertyNames[1]}",
            _ => $"{string.Join(", ", propertyNames.Take(propertyNames.Length - 1))}, or {propertyNames[^1]}",
        };

    private static void GenerateProperty(
        StringBuilder sb,
        CliOptionDefinition option,
        bool requiredPropertiesAreNonNullable)
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
        var accessor = GetPropertyAccessor(option.IsRequired);
        var propertyType = option.IsRequired && requiredPropertiesAreNonNullable && !RequiresNullableFlagProperty(option)
            ? option.PropertyType.TrimEnd('?')
            : option.PropertyType;
        sb.AppendLine($"    public {GetNewModifier(option.PropertyName)}{propertyType} {option.PropertyName} {{ get; {accessor}; }}");
    }

    private static void GeneratePositionalArgument(
        StringBuilder sb,
        CliPositionalArgument positional,
        bool requiredPropertiesAreNonNullable)
    {
        GeneratorUtils.GenerateXmlDocumentation(sb, positional.Description);

        if (positional.IsSecret)
        {
            sb.AppendLine("    [SecretValue]");
        }

        var attrString = GetPositionalAttributeString(positional);
        sb.AppendLine($"    [{attrString}]");
        var accessor = GetPropertyAccessor(positional.IsRequired);
        var propertyType = positional.CSharpType;
        if (positional.IsValidationRequired == false)
        {
            propertyType = propertyType.TrimEnd('?') + "?";
        }
        else if (positional.IsRequired && requiredPropertiesAreNonNullable)
        {
            propertyType = propertyType.TrimEnd('?');
        }

        sb.AppendLine($"    public {propertyType} {positional.PropertyName} {{ get; {accessor}; }}");
    }

    private static string GetPropertyAccessor(bool isRequired) =>
        isRequired ? "private init" : "set";

    private static string GetNewModifier(string propertyName) =>
        InheritedPropertyCollisionResolver.IsInheritedPropertyName(propertyName) ? "new " : "";

    private static string GetPositionalAttributeString(CliPositionalArgument positional)
    {
        var parts = new List<string>
        {
            positional.PositionIndex.ToString(),
            $"Phase = CommandLinePhase.{positional.Phase}"
        };

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
