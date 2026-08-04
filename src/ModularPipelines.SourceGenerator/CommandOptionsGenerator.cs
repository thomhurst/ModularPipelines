using System.Collections.Immutable;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ModularPipelines.SourceGenerator;

/// <summary>
/// Generates direct property-access metadata for CLI options and secrets.
/// </summary>
[Generator]
public sealed class CommandOptionsGenerator : IIncrementalGenerator
{
    private const string CliValuePairFullName = "ModularPipelines.Models.CliValuePair";

    internal const string CommandLineToolOptionsFullName = "ModularPipelines.Options.CommandLineToolOptions";
    internal const string CliOptionAttributeFullName = "ModularPipelines.Attributes.CliOptionAttribute";
    internal const string CliFlagAttributeFullName = "ModularPipelines.Attributes.CliFlagAttribute";
    internal const string CliArgumentAttributeFullName = "ModularPipelines.Attributes.CliArgumentAttribute";
    internal const string CliGlobalOptionsAttributeFullName = "ModularPipelines.Attributes.CliGlobalOptionsAttribute";
    internal const string SecretValueAttributeFullName = "ModularPipelines.Attributes.SecretValueAttribute";

    private static readonly DiagnosticDescriptor IncompleteCommandMetadata =
        GeneratorDiagnostics.IncompleteCommandMetadata;

    private static readonly DiagnosticDescriptor IncompleteSecretMetadata =
        GeneratorDiagnostics.IncompleteSecretMetadata;

    private static readonly DiagnosticDescriptor SkippedRuntimeMetadata =
        GeneratorDiagnostics.SkippedRuntimeMetadata;

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var typeCandidates = context.SyntaxProvider
            .CreateSyntaxProvider(
                static (node, _) => IsTypeCandidate(node),
                static (generatorContext, _) => GetTypeCandidate(generatorContext))
            .Where(static item => item is not null)
            .Select(static (item, _) => item!)
            .WithComparer(TypeMetadataCandidateComparer.Instance);

        var secretCandidates = context.SyntaxProvider
            .ForAttributeWithMetadataName(
                SecretValueAttributeFullName,
                static (_, _) => true,
                static (generatorContext, _) => GetTypeCandidate(generatorContext))
            .Where(static item => item is not null)
            .Select(static (item, _) => item!)
            .WithComparer(TypeMetadataCandidateComparer.Instance);

        var candidates = typeCandidates.Collect().Combine(secretCandidates.Collect());
        context.RegisterSourceOutput(candidates, static (sourceContext, candidateGroups) =>
        {
            var candidates = candidateGroups.Left.AddRange(candidateGroups.Right);
            foreach (var skipped in candidates
                         .Where(static candidate => candidate.Metadata is null)
                         .GroupBy(static candidate => candidate.TypeName, StringComparer.Ordinal)
                         .Select(static group => group.First()))
            {
                sourceContext.ReportDiagnostic(Diagnostic.Create(
                    SkippedRuntimeMetadata,
                    skipped.Location,
                    skipped.TypeName));
            }

            var items = candidates
                .Select(static candidate => candidate.Metadata)
                .OfType<TypeMetadata>()
                .ToImmutableArray();
            if (items.Length > 0)
            {
                ReportIncompleteMetadata(sourceContext, candidates);
                sourceContext.AddSource("ModularPipelines.RuntimeMetadata.g.cs", Generate(items));
            }
        });
    }

    internal static bool IsTypeCandidate(SyntaxNode node)
    {
        return node is ClassDeclarationSyntax { BaseList.Types.Count: > 0 }
            || (node is RecordDeclarationSyntax record
                && !record.ClassOrStructKeyword.IsKind(SyntaxKind.StructKeyword)
                && (record.BaseList is { Types.Count: > 0 }
                    || record.ParameterList?.Parameters.Any(
                        static parameter => parameter.AttributeLists.Count > 0) == true));
    }

    private static TypeMetadataCandidate? GetTypeCandidate(GeneratorSyntaxContext context)
    {
        return GetTypeCandidate(
            context.SemanticModel.GetDeclaredSymbol(context.Node) as INamedTypeSymbol,
            context.SemanticModel.Compilation,
            hasKnownSecretAttribute: false);
    }

    private static TypeMetadataCandidate? GetTypeCandidate(GeneratorAttributeSyntaxContext context)
    {
        return GetTypeCandidate(
            context.TargetSymbol.ContainingType,
            context.SemanticModel.Compilation,
            hasKnownSecretAttribute: true);
    }

    private static TypeMetadataCandidate? GetTypeCandidate(
        INamedTypeSymbol? type,
        Compilation compilation,
        bool hasKnownSecretAttribute)
    {
        if (type is null)
        {
            return null;
        }

        var isCommandOptions = InheritsFrom(type, CommandLineToolOptionsFullName);
        var typeName = type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
        var location = type.Locations.FirstOrDefault() ?? Location.None;
        if (!IsTypeAccessible(type, compilation.Assembly))
        {
            return isCommandOptions || hasKnownSecretAttribute
                ? new TypeMetadataCandidate(typeName, location, Metadata: null)
                : null;
        }

        if (type.IsGenericType)
        {
            return isCommandOptions || hasKnownSecretAttribute
                ? new TypeMetadataCandidate(typeName, location, Metadata: null)
                : null;
        }

        var commandMetadata = isCommandOptions
            ? GetCommandProperties(type, compilation.Assembly)
            : PropertyCollection.Empty;
        var secretMetadata = GetSecretProperties(type, compilation.Assembly);
        if (!isCommandOptions && !secretMetadata.HasAttributes)
        {
            return null;
        }

        return new TypeMetadataCandidate(typeName, location, new TypeMetadata(
            typeName,
            isCommandOptions,
            commandMetadata,
            secretMetadata));
    }

    private static PropertyCollection GetCommandProperties(
        INamedTypeSymbol type,
        IAssemblySymbol currentAssembly)
    {
        var properties = new Dictionary<string, PropertyMetadata>(StringComparer.Ordinal);
        var seenPropertyNames = new HashSet<string>(StringComparer.Ordinal);
        var isComplete = true;
        var hasAttributes = false;

        for (var current = type; current is not null; current = current.BaseType)
        {
            foreach (var property in current.GetMembers().OfType<IPropertySymbol>())
            {
                if (property.IsStatic || property.GetMethod is null || !seenPropertyNames.Add(property.Name))
                {
                    continue;
                }

                var attribute = FindAttribute(property, IsCommandAttribute);
                if (attribute is null)
                {
                    continue;
                }

                hasAttributes = true;
                if (!IsPropertyAccessible(property, currentAssembly))
                {
                    isComplete = false;
                    continue;
                }

                var propertyMetadata = CreatePropertyMetadata(property, attribute);
                if (propertyMetadata is
                    {
                        Kind: PropertyKind.Flag or PropertyKind.Option,
                        PrimaryValue: null,
                    })
                {
                    isComplete = false;
                    continue;
                }

                properties.Add(property.Name, propertyMetadata);
            }
        }

        return new PropertyCollection(properties.Values.ToImmutableArray(), isComplete, hasAttributes);
    }

    private static PropertyCollection GetSecretProperties(
        INamedTypeSymbol type,
        IAssemblySymbol currentAssembly)
    {
        var properties = ImmutableArray.CreateBuilder<PropertyMetadata>();
        var seenPropertyNames = new HashSet<string>(StringComparer.Ordinal);
        var isComplete = true;
        var hasAttributes = false;

        for (var current = type; current is not null; current = current.BaseType)
        {
            foreach (var property in current.GetMembers().OfType<IPropertySymbol>())
            {
                if (property.IsStatic || property.GetMethod is null || !seenPropertyNames.Add(property.Name))
                {
                    continue;
                }

                var secretAttribute = FindAttribute(property, IsSecretAttribute);
                if (secretAttribute is null)
                {
                    continue;
                }

                hasAttributes = true;
                if (!IsAttribute(secretAttribute, SecretValueAttributeFullName)
                    || !IsPropertyAccessible(property, currentAssembly))
                {
                    isComplete = false;
                    continue;
                }

                properties.Add(new PropertyMetadata(
                    property.Name,
                    PropertyKind.Secret,
                    null,
                    null,
                    false,
                    false,
                    false,
                    0,
                    "Normal",
                    0,
                    GetConstructorStrings(secretAttribute),
                    false,
                    false,
                    0));
            }
        }

        return new PropertyCollection(properties.ToImmutable(), isComplete, hasAttributes);
    }

    private static AttributeData? FindAttribute(
        IPropertySymbol property,
        Func<AttributeData, bool> predicate)
    {
        for (var current = property; current is not null; current = current.OverriddenProperty)
        {
            var attribute = current.GetAttributes().FirstOrDefault(predicate);
            if (attribute is not null)
            {
                return attribute;
            }
        }

        return null;
    }

    private static PropertyMetadata CreatePropertyMetadata(IPropertySymbol property, AttributeData attribute)
    {
        var isGlobalOption = IsGlobalOption(property);
        var attributeName = attribute.AttributeClass?.ToDisplayString();
        if (attributeName == CliArgumentAttributeFullName)
        {
            return new PropertyMetadata(
                property.Name,
                PropertyKind.Argument,
                null,
                null,
                GetNamedBool(attribute, "PrependOptionTerminator"),
                false,
                GetNamedBool(attribute, "Required"),
                GetConstructorInt(attribute),
                GetNamedEnumMemberName(attribute, "Phase", "Passthrough"),
                0,
                EquatableArray<string>.Empty,
                GetNamedBool(attribute, "PrependOptionTerminatorIfValueStartsWithDash"),
                isGlobalOption,
                0);
        }

        if (attributeName == CliFlagAttributeFullName)
        {
            return new PropertyMetadata(
                property.Name,
                PropertyKind.Flag,
                GetConstructorString(attribute),
                GetNamedString(attribute, "ShortForm"),
                GetNamedBool(attribute, "PreferShortForm"),
                false,
                false,
                0,
                GetNamedEnumMemberName(attribute, "Phase", "Normal"),
                0,
                EquatableArray<string>.Empty,
                false,
                isGlobalOption,
                0);
        }

        return new PropertyMetadata(
            property.Name,
            PropertyKind.Option,
            GetConstructorString(attribute),
            GetNamedString(attribute, "ShortForm"),
            GetNamedBool(attribute, "PreferShortForm"),
            GetNamedBool(attribute, "GroupValues"),
            false,
            GetNamedInt(attribute, "Format"),
            GetNamedEnumMemberName(attribute, "Phase", "Normal"),
            GetNamedInt(attribute, "ValueArity"),
            EquatableArray<string>.Empty,
            false,
            isGlobalOption,
            GetManualOperandCount(property.Type));
    }

    private static int GetManualOperandCount(ITypeSymbol propertyType)
    {
        if (propertyType is INamedTypeSymbol nullableType
            && nullableType.OriginalDefinition.SpecialType == SpecialType.System_Nullable_T
            && nullableType.TypeArguments.Length == 1)
        {
            propertyType = nullableType.TypeArguments[0];
        }

        if (propertyType.ToDisplayString() == CliValuePairFullName)
        {
            return 2;
        }

        return propertyType is INamedTypeSymbol namedType
               && namedType.AllInterfaces
                   .Append(namedType)
                   .Any(type => type.OriginalDefinition.SpecialType
                                == SpecialType.System_Collections_Generic_IEnumerable_T
                                && type.TypeArguments[0].ToDisplayString() == CliValuePairFullName)
            ? 2
            : 1;
    }

    private static bool IsGlobalOption(IPropertySymbol property)
    {
        for (var current = property; current is not null; current = current.OverriddenProperty)
        {
            if (current.ContainingType.GetAttributes()
                .Any(candidate => IsAttribute(candidate, CliGlobalOptionsAttributeFullName)))
            {
                return true;
            }
        }

        return false;
    }

    private static string Generate(ImmutableArray<TypeMetadata> items)
    {
        var uniqueItems = items
            .GroupBy(item => item.TypeName, StringComparer.Ordinal)
            .Select(group => group.First())
            .OrderBy(item => item.TypeName, StringComparer.Ordinal)
            .ToList();
        var sb = new StringBuilder();

        sb.AppendLine("// <auto-generated/>");
        sb.AppendLine("#nullable enable");
        sb.AppendLine();
        sb.AppendLine("namespace ModularPipelines.Generated;");
        sb.AppendLine();
        sb.AppendLine("internal static class RuntimeMetadataRegistration");
        sb.AppendLine("{");
        sb.AppendLine("    [global::System.Runtime.CompilerServices.ModuleInitializer]");
        sb.AppendLine("    internal static void Register()");
        sb.AppendLine("    {");

        foreach (var item in uniqueItems)
        {
            if (item.IsCommandOptions)
            {
                AppendCommandRegistration(sb, item);
            }

            if (item.SecretMetadata.HasAttributes)
            {
                AppendSecretRegistration(sb, item);
            }
        }

        sb.AppendLine("    }");
        sb.AppendLine("}");
        return sb.ToString();
    }

    private static void ReportIncompleteMetadata(
        SourceProductionContext context,
        IReadOnlyCollection<TypeMetadataCandidate> candidates)
    {
        foreach (var candidate in candidates
                     .Where(static candidate => candidate.Metadata is not null)
                     .GroupBy(static candidate => candidate.TypeName, StringComparer.Ordinal)
                     .Select(static group => group.First()))
        {
            var item = candidate.Metadata!;
            if (!item.CommandMetadata.IsComplete)
            {
                context.ReportDiagnostic(Diagnostic.Create(
                    IncompleteCommandMetadata,
                    candidate.Location,
                    item.TypeName));
            }

            if (!item.SecretMetadata.IsComplete)
            {
                context.ReportDiagnostic(Diagnostic.Create(
                    IncompleteSecretMetadata,
                    candidate.Location,
                    item.TypeName));
            }
        }
    }

    private static void AppendCommandRegistration(StringBuilder sb, TypeMetadata item)
    {
        sb.AppendLine("        global::ModularPipelines.Helpers.Internal.GeneratedCommandMetadata.Register(");
        sb.AppendLine($"            typeof({item.TypeName}),");
        sb.AppendLine("            new global::ModularPipelines.Helpers.Internal.PropertyCommandLinePart[]");
        sb.AppendLine("            {");

        foreach (var property in item.CommandMetadata.Properties)
        {
            var getter = $"static instance => (({item.TypeName})instance).@{property.Name}";
            switch (property.Kind)
            {
                case PropertyKind.Argument:
                    sb.AppendLine("                new global::ModularPipelines.Helpers.Internal.ArgumentPart(");
                    sb.AppendLine($"                    {Literal(property.Name)}, {getter},");
                    sb.AppendLine($"                    new global::ModularPipelines.Attributes.CliArgumentAttribute({property.FirstInt})");
                    sb.AppendLine("                    {");
                    sb.AppendLine($"                        Phase = global::ModularPipelines.Attributes.CommandLinePhase.{property.Phase},");
                    sb.AppendLine($"                        PrependOptionTerminator = {BooleanLiteral(property.BooleanValue)},");
                    sb.AppendLine($"                        PrependOptionTerminatorIfValueStartsWithDash = {BooleanLiteral(property.PrependOptionTerminatorIfValueStartsWithDash)},");
                    sb.AppendLine($"                        Required = {BooleanLiteral(property.IsRequired)},");
                    sb.AppendLine($"                    }}) {{ IsGlobalOption = {BooleanLiteral(property.IsGlobalOption)} }},");
                    break;
                case PropertyKind.Flag:
                    sb.AppendLine("                new global::ModularPipelines.Helpers.Internal.FlagPart(");
                    sb.AppendLine($"                    {Literal(property.Name)}, {getter},");
                    sb.AppendLine($"                    new global::ModularPipelines.Attributes.CliFlagAttribute({Literal(property.PrimaryValue!)})");
                    sb.AppendLine("                    {");
                    sb.AppendLine($"                        ShortForm = {NullableLiteral(property.ShortForm)},");
                    sb.AppendLine($"                        PreferShortForm = {BooleanLiteral(property.BooleanValue)},");
                    sb.AppendLine($"                        Phase = global::ModularPipelines.Attributes.CommandLinePhase.{property.Phase},");
                    sb.AppendLine($"                    }}) {{ IsGlobalOption = {BooleanLiteral(property.IsGlobalOption)} }},");
                    break;
                case PropertyKind.Option:
                    sb.AppendLine("                new global::ModularPipelines.Helpers.Internal.OptionPart(");
                    sb.AppendLine($"                    {Literal(property.Name)}, {getter},");
                    sb.AppendLine($"                    new global::ModularPipelines.Attributes.CliOptionAttribute({Literal(property.PrimaryValue!)})");
                    sb.AppendLine("                    {");
                    sb.AppendLine($"                        ShortForm = {NullableLiteral(property.ShortForm)},");
                    sb.AppendLine($"                        PreferShortForm = {BooleanLiteral(property.BooleanValue)},");
                    sb.AppendLine($"                        Format = (global::ModularPipelines.Attributes.OptionFormat){property.FirstInt},");
                    sb.AppendLine($"                        ValueArity = (global::ModularPipelines.Attributes.CliOptionValueArity){property.ValueArity},");
                    sb.AppendLine($"                        GroupValues = {BooleanLiteral(property.GroupValues)},");
                    sb.AppendLine($"                        Phase = global::ModularPipelines.Attributes.CommandLinePhase.{property.Phase},");
                    sb.AppendLine($"                    }}) {{ IsGlobalOption = {BooleanLiteral(property.IsGlobalOption)}, ManualOperandCount = {property.ManualOperandCount} }},");
                    break;
            }
        }

        sb.AppendLine($"            }}, isComplete: {BooleanLiteral(item.CommandMetadata.IsComplete)});");
    }

    private static void AppendSecretRegistration(StringBuilder sb, TypeMetadata item)
    {
        sb.AppendLine("        global::ModularPipelines.Engine.GeneratedSecretMetadata.Register(");
        sb.AppendLine($"            typeof({item.TypeName}),");
        sb.AppendLine("            new global::ModularPipelines.Engine.SecretPropertyAccessor[]");
        sb.AppendLine("            {");

        foreach (var property in item.SecretMetadata.Properties)
        {
            sb.AppendLine($"                new({Literal(property.Name)}, static instance => (({item.TypeName})instance).@{property.Name}, {StringArrayLiteral(property.SecretValueKeys)}),");
        }

        sb.AppendLine($"            }}, isComplete: {BooleanLiteral(item.SecretMetadata.IsComplete)});");
    }

    private static bool InheritsFrom(INamedTypeSymbol type, string metadataName)
    {
        for (var current = type; current is not null; current = current.BaseType)
        {
            if (current.ToDisplayString() == metadataName)
            {
                return true;
            }
        }

        return false;
    }

    private static bool IsTypeAccessible(INamedTypeSymbol type, IAssemblySymbol currentAssembly)
    {
        for (var current = type; current is not null; current = current.ContainingType)
        {
            if (!IsAccessible(current.DeclaredAccessibility, current.ContainingAssembly, currentAssembly))
            {
                return false;
            }
        }

        return true;
    }

    private static bool IsPropertyAccessible(IPropertySymbol property, IAssemblySymbol currentAssembly)
    {
        return IsAccessible(property.DeclaredAccessibility, property.ContainingAssembly, currentAssembly)
            && IsAccessible(property.GetMethod!.DeclaredAccessibility, property.ContainingAssembly, currentAssembly)
            && IsTypeAccessible(property.ContainingType, currentAssembly);
    }

    private static bool IsAccessible(
        Accessibility accessibility,
        IAssemblySymbol containingAssembly,
        IAssemblySymbol currentAssembly)
    {
        return accessibility == Accessibility.Public
            || ((accessibility == Accessibility.Internal || accessibility == Accessibility.ProtectedOrInternal)
                && (SymbolEqualityComparer.Default.Equals(containingAssembly, currentAssembly)
                    || containingAssembly.GivesAccessTo(currentAssembly)));
    }

    private static bool IsCommandAttribute(AttributeData attribute)
    {
        var name = attribute.AttributeClass?.ToDisplayString();
        return name == CliArgumentAttributeFullName || name == CliFlagAttributeFullName || name == CliOptionAttributeFullName;
    }

    private static bool IsSecretAttribute(AttributeData attribute) =>
        attribute.AttributeClass is { } attributeClass
        && InheritsFrom(attributeClass, SecretValueAttributeFullName);

    private static bool IsAttribute(AttributeData attribute, string fullName) =>
        attribute.AttributeClass?.ToDisplayString() == fullName;

    private static string? GetConstructorString(AttributeData attribute) =>
        attribute.ConstructorArguments.FirstOrDefault().Value as string;

    private static int GetConstructorInt(AttributeData attribute) =>
        attribute.ConstructorArguments.Length == 0 ? 0 : Convert.ToInt32(attribute.ConstructorArguments[0].Value);

    private static ImmutableArray<string> GetConstructorStrings(AttributeData attribute)
    {
        if (attribute.ConstructorArguments.Length == 0 ||
            attribute.ConstructorArguments[0].Kind != TypedConstantKind.Array)
        {
            return [];
        }

        return
        [
            .. attribute.ConstructorArguments[0].Values
                .Select(value => value.Value)
                .OfType<string>(),
        ];
    }

    private static string? GetNamedString(AttributeData attribute, string name) =>
        attribute.NamedArguments.FirstOrDefault(argument => argument.Key == name).Value.Value as string;

    private static bool GetNamedBool(AttributeData attribute, string name) =>
        attribute.NamedArguments.FirstOrDefault(argument => argument.Key == name).Value.Value as bool? ?? false;

    private static int GetNamedInt(AttributeData attribute, string name, int defaultValue = 0)
    {
        var value = attribute.NamedArguments.FirstOrDefault(argument => argument.Key == name).Value.Value;
        return value is null ? defaultValue : Convert.ToInt32(value);
    }

    private static string GetNamedEnumMemberName(
        AttributeData attribute,
        string name,
        string defaultValue)
    {
        var typedConstant = attribute.NamedArguments
            .FirstOrDefault(argument => argument.Key == name)
            .Value;
        if (typedConstant.Value is null || typedConstant.Type is null)
        {
            return defaultValue;
        }

        var numericValue = Convert.ToInt64(typedConstant.Value);
        return typedConstant.Type
                   .GetMembers()
                   .OfType<IFieldSymbol>()
                   .FirstOrDefault(field => field.HasConstantValue
                                            && Convert.ToInt64(field.ConstantValue) == numericValue)
                   ?.Name
               ?? defaultValue;
    }

    private static string BooleanLiteral(bool value) => value ? "true" : "false";

    private static string NullableLiteral(string? value) => value is null ? "null" : Literal(value);

    private static string StringArrayLiteral(EquatableArray<string> values) =>
        values.Count == 0
            ? "global::System.Array.Empty<string>()"
            : $"new string[] {{ {string.Join(", ", values.Select(Literal))} }}";

    private static string Literal(string value) =>
        global::Microsoft.CodeAnalysis.CSharp.SymbolDisplay.FormatLiteral(value, quote: true);

    private sealed record TypeMetadata(
        string TypeName,
        bool IsCommandOptions,
        PropertyCollection CommandMetadata,
        PropertyCollection SecretMetadata);

    private sealed record TypeMetadataCandidate(
        string TypeName,
        Location Location,
        TypeMetadata? Metadata);

    private sealed class TypeMetadataCandidateComparer : IEqualityComparer<TypeMetadataCandidate>
    {
        public static TypeMetadataCandidateComparer Instance { get; } = new();

        public bool Equals(TypeMetadataCandidate? x, TypeMetadataCandidate? y) =>
            ReferenceEquals(x, y)
            || (x is not null
                && y is not null
                && StringComparer.Ordinal.Equals(x.TypeName, y.TypeName)
                && EqualityComparer<TypeMetadata?>.Default.Equals(x.Metadata, y.Metadata)
                && (!RequiresDiagnostic(x) || x.Location.Equals(y.Location)));

        public int GetHashCode(TypeMetadataCandidate obj)
        {
            var hashCode = (StringComparer.Ordinal.GetHashCode(obj.TypeName) * 397)
                           ^ (obj.Metadata?.GetHashCode() ?? 0);
            return RequiresDiagnostic(obj)
                ? (hashCode * 397) ^ obj.Location.GetHashCode()
                : hashCode;
        }

        private static bool RequiresDiagnostic(TypeMetadataCandidate candidate) =>
            candidate.Metadata is null
            || !candidate.Metadata.CommandMetadata.IsComplete
            || !candidate.Metadata.SecretMetadata.IsComplete;
    }

    private sealed record PropertyCollection(
        EquatableArray<PropertyMetadata> Properties,
        bool IsComplete,
        bool HasAttributes)
    {
        public static PropertyCollection Empty { get; } = new(EquatableArray<PropertyMetadata>.Empty, true, false);
    }

    private sealed record PropertyMetadata(
        string Name,
        PropertyKind Kind,
        string? PrimaryValue,
        string? ShortForm,
        bool BooleanValue,
        bool GroupValues,
        bool IsRequired,
        int FirstInt,
        string Phase,
        int ValueArity,
        EquatableArray<string> SecretValueKeys,
        bool PrependOptionTerminatorIfValueStartsWithDash,
        bool IsGlobalOption,
        int ManualOperandCount);

    private enum PropertyKind
    {
        Argument,
        Flag,
        Option,
        Secret,
    }
}
