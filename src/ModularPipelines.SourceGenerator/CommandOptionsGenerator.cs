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
    internal const string CommandLineToolOptionsFullName = "ModularPipelines.Options.CommandLineToolOptions";
    internal const string CliOptionAttributeFullName = "ModularPipelines.Attributes.CliOptionAttribute";
    internal const string CliFlagAttributeFullName = "ModularPipelines.Attributes.CliFlagAttribute";
    internal const string CliArgumentAttributeFullName = "ModularPipelines.Attributes.CliArgumentAttribute";
    internal const string SecretValueAttributeFullName = "ModularPipelines.Attributes.SecretValueAttribute";

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var metadata = context.SyntaxProvider
            .CreateSyntaxProvider(
                static (node, _) => node is ClassDeclarationSyntax
                    || (node is RecordDeclarationSyntax record
                        && !record.ClassOrStructKeyword.IsKind(SyntaxKind.StructKeyword)),
                static (generatorContext, _) => GetTypeMetadata(generatorContext))
            .Where(static item => item is not null)
            .Select(static (item, _) => item!);

        context.RegisterSourceOutput(metadata.Collect(), static (sourceContext, items) =>
        {
            if (items.Length > 0)
            {
                sourceContext.AddSource("ModularPipelines.RuntimeMetadata.g.cs", Generate(items));
            }
        });
    }

    private static TypeMetadata? GetTypeMetadata(GeneratorSyntaxContext context)
    {
        if (context.SemanticModel.GetDeclaredSymbol(context.Node) is not INamedTypeSymbol type
            || type.IsGenericType
            || !IsTypeAccessible(type, context.SemanticModel.Compilation.Assembly))
        {
            return null;
        }

        var isCommandOptions = InheritsFrom(type, CommandLineToolOptionsFullName);
        var commandMetadata = isCommandOptions
            ? GetCommandProperties(type, context.SemanticModel.Compilation.Assembly)
            : PropertyCollection.Empty;
        var secretMetadata = GetSecretProperties(type, context.SemanticModel.Compilation.Assembly);

        if (!isCommandOptions && !secretMetadata.HasAttributes)
        {
            return null;
        }

        return new TypeMetadata(
            type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat),
            isCommandOptions,
            commandMetadata,
            secretMetadata);
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

                properties.Add(property.Name, CreatePropertyMetadata(property, attribute));
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
                if (property.IsStatic || property.GetMethod is null || !seenPropertyNames.Add(property.Name)
                    || FindAttribute(property, attribute => IsAttribute(attribute, SecretValueAttributeFullName)) is null)
                {
                    continue;
                }

                hasAttributes = true;
                if (!IsPropertyAccessible(property, currentAssembly))
                {
                    isComplete = false;
                    continue;
                }

                var secretAttribute = FindAttribute(property, attribute => IsAttribute(attribute, SecretValueAttributeFullName))!;
                properties.Add(new PropertyMetadata(
                    property.Name,
                    PropertyKind.Secret,
                    null,
                    null,
                    false,
                    0,
                    0,
                    null,
                    GetConstructorStrings(secretAttribute)));
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
        var attributeName = attribute.AttributeClass?.ToDisplayString();
        if (attributeName == CliArgumentAttributeFullName)
        {
            return new PropertyMetadata(
                property.Name,
                PropertyKind.Argument,
                GetNamedString(attribute, "Name"),
                null,
                false,
                GetConstructorInt(attribute),
                GetNamedInt(attribute, "Placement"),
                null,
                []);
        }

        if (attributeName == CliFlagAttributeFullName)
        {
            return new PropertyMetadata(
                property.Name,
                PropertyKind.Flag,
                GetConstructorString(attribute),
                GetNamedString(attribute, "ShortForm"),
                GetNamedBool(attribute, "PreferShortForm"),
                0,
                0,
                null,
                []);
        }

        return new PropertyMetadata(
            property.Name,
            PropertyKind.Option,
            GetConstructorString(attribute),
            GetNamedString(attribute, "ShortForm"),
            GetNamedBool(attribute, "PreferShortForm"),
            GetNamedInt(attribute, "Format"),
            GetNamedBool(attribute, "AllowMultiple") ? 1 : 0,
            GetNamedString(attribute, "CustomSeparator"),
            []);
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
                    sb.AppendLine($"                        Placement = (global::ModularPipelines.Attributes.ArgumentPlacement){property.SecondInt},");
                    sb.AppendLine($"                        Name = {NullableLiteral(property.PrimaryValue)},");
                    sb.AppendLine("                    }),");
                    break;
                case PropertyKind.Flag:
                    sb.AppendLine("                new global::ModularPipelines.Helpers.Internal.FlagPart(");
                    sb.AppendLine($"                    {Literal(property.Name)}, {getter},");
                    sb.AppendLine($"                    new global::ModularPipelines.Attributes.CliFlagAttribute({Literal(property.PrimaryValue!)})");
                    sb.AppendLine("                    {");
                    sb.AppendLine($"                        ShortForm = {NullableLiteral(property.ShortForm)},");
                    sb.AppendLine($"                        PreferShortForm = {BooleanLiteral(property.BooleanValue)},");
                    sb.AppendLine("                    }),");
                    break;
                case PropertyKind.Option:
                    sb.AppendLine("                new global::ModularPipelines.Helpers.Internal.OptionPart(");
                    sb.AppendLine($"                    {Literal(property.Name)}, {getter},");
                    sb.AppendLine($"                    new global::ModularPipelines.Attributes.CliOptionAttribute({Literal(property.PrimaryValue!)})");
                    sb.AppendLine("                    {");
                    sb.AppendLine($"                        ShortForm = {NullableLiteral(property.ShortForm)},");
                    sb.AppendLine($"                        PreferShortForm = {BooleanLiteral(property.BooleanValue)},");
                    sb.AppendLine($"                        Format = (global::ModularPipelines.Attributes.OptionFormat){property.FirstInt},");
                    sb.AppendLine($"                        AllowMultiple = {BooleanLiteral(property.SecondInt == 1)},");
                    sb.AppendLine($"                        CustomSeparator = {NullableLiteral(property.CustomSeparator)},");
                    sb.AppendLine("                    }),");
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
                && SymbolEqualityComparer.Default.Equals(containingAssembly, currentAssembly));
    }

    private static bool IsCommandAttribute(AttributeData attribute)
    {
        var name = attribute.AttributeClass?.ToDisplayString();
        return name == CliArgumentAttributeFullName || name == CliFlagAttributeFullName || name == CliOptionAttributeFullName;
    }

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

        return attribute.ConstructorArguments[0].Values
            .Select(value => value.Value as string)
            .Where(value => value is not null)
            .Cast<string>()
            .ToImmutableArray();
    }

    private static string? GetNamedString(AttributeData attribute, string name) =>
        attribute.NamedArguments.FirstOrDefault(argument => argument.Key == name).Value.Value as string;

    private static bool GetNamedBool(AttributeData attribute, string name) =>
        attribute.NamedArguments.FirstOrDefault(argument => argument.Key == name).Value.Value as bool? ?? false;

    private static int GetNamedInt(AttributeData attribute, string name)
    {
        var value = attribute.NamedArguments.FirstOrDefault(argument => argument.Key == name).Value.Value;
        return value is null ? 0 : Convert.ToInt32(value);
    }

    private static string BooleanLiteral(bool value) => value ? "true" : "false";

    private static string NullableLiteral(string? value) => value is null ? "null" : Literal(value);

    private static string StringArrayLiteral(ImmutableArray<string> values) =>
        values.Length == 0
            ? "global::System.Array.Empty<string>()"
            : $"new string[] {{ {string.Join(", ", values.Select(Literal))} }}";

    private static string Literal(string value) =>
        global::Microsoft.CodeAnalysis.CSharp.SymbolDisplay.FormatLiteral(value, quote: true);

    private sealed record TypeMetadata(
        string TypeName,
        bool IsCommandOptions,
        PropertyCollection CommandMetadata,
        PropertyCollection SecretMetadata);

    private sealed record PropertyCollection(
        ImmutableArray<PropertyMetadata> Properties,
        bool IsComplete,
        bool HasAttributes)
    {
        public static PropertyCollection Empty { get; } = new([], true, false);
    }

    private sealed record PropertyMetadata(
        string Name,
        PropertyKind Kind,
        string? PrimaryValue,
        string? ShortForm,
        bool BooleanValue,
        int FirstInt,
        int SecondInt,
        string? CustomSeparator,
        ImmutableArray<string> SecretValueKeys);

    private enum PropertyKind
    {
        Argument,
        Flag,
        Option,
        Secret,
    }
}
