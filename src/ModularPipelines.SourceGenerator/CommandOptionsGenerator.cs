using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ModularPipelines.SourceGenerator;

/// <summary>
/// Source generator that validates CommandLineToolOptions classes
/// and generates optimized Build() methods.
/// </summary>
[Generator]
public sealed class CommandOptionsGenerator : IIncrementalGenerator
{
    /// <summary>
    /// The fully qualified name of the CommandLineToolOptions base class.
    /// </summary>
    internal const string CommandLineToolOptionsFullName = "ModularPipelines.Options.CommandLineToolOptions";

    /// <summary>
    /// The fully qualified name of the CliToolAttribute.
    /// </summary>
    internal const string CliToolAttributeFullName = "ModularPipelines.Attributes.CliToolAttribute";

    /// <summary>
    /// The fully qualified name of the CliSubCommandAttribute.
    /// </summary>
    internal const string CliSubCommandAttributeFullName = "ModularPipelines.Attributes.CliSubCommandAttribute";

    /// <summary>
    /// The fully qualified name of the CliOptionAttribute.
    /// </summary>
    internal const string CliOptionAttributeFullName = "ModularPipelines.Attributes.CliOptionAttribute";

    /// <summary>
    /// The fully qualified name of the CliFlagAttribute.
    /// </summary>
    internal const string CliFlagAttributeFullName = "ModularPipelines.Attributes.CliFlagAttribute";

    /// <summary>
    /// The fully qualified name of the CliArgumentAttribute.
    /// </summary>
    internal const string CliArgumentAttributeFullName = "ModularPipelines.Attributes.CliArgumentAttribute";

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        // Create syntax provider that finds record/class declarations
        var optionsClasses = context.SyntaxProvider
            .CreateSyntaxProvider(
                predicate: static (node, _) => IsCandidate(node),
                transform: static (ctx, _) => GetOptionsClassInfo(ctx))
            .Where(static info => info is not null)
            .Select(static (info, _) => info!);

        // Register the output
        context.RegisterSourceOutput(optionsClasses, static (ctx, info) => GenerateCode(ctx, info));
    }

    /// <summary>
    /// Checks if a syntax node is a potential candidate for options class discovery.
    /// Returns true for record or class declarations.
    /// </summary>
    private static bool IsCandidate(SyntaxNode node)
    {
        return node is RecordDeclarationSyntax || node is ClassDeclarationSyntax;
    }

    /// <summary>
    /// Extracts OptionsClassInfo from a type declaration if it inherits from CommandLineToolOptions.
    /// </summary>
    private static OptionsClassInfo? GetOptionsClassInfo(GeneratorSyntaxContext context)
    {
        var typeDeclaration = (TypeDeclarationSyntax)context.Node;
        var semanticModel = context.SemanticModel;

        // Get the declared symbol for this type
        if (semanticModel.GetDeclaredSymbol(typeDeclaration) is not INamedTypeSymbol typeSymbol)
        {
            return null;
        }

        // Check if this type inherits from CommandLineToolOptions
        if (!InheritsFromCommandLineToolOptions(typeSymbol, semanticModel.Compilation))
        {
            return null;
        }

        // Extract namespace
        var namespaceName = typeSymbol.ContainingNamespace.IsGlobalNamespace
            ? string.Empty
            : typeSymbol.ContainingNamespace.ToDisplayString();

        // Check if the type is partial
        var isPartial = typeDeclaration.Modifiers.Any(SyntaxKind.PartialKeyword);

        // Get the tool name from the type hierarchy
        var toolName = GetToolName(typeSymbol);

        // Get subcommand parts (stub for now - Task 2.3)
        var subCommandParts = GetSubCommandParts(typeSymbol);

        // Get CLI properties (stub for now - Task 2.3)
        var properties = GetCliProperties(typeSymbol);

        return new OptionsClassInfo(
            Namespace: namespaceName,
            ClassName: typeSymbol.Name,
            ToolName: toolName,
            SubCommandParts: subCommandParts,
            Properties: properties,
            IsPartial: isPartial,
            Location: typeDeclaration.Identifier.GetLocation()
        );
    }

    /// <summary>
    /// Checks if the given type inherits from CommandLineToolOptions.
    /// Walks the base type hierarchy.
    /// </summary>
    private static bool InheritsFromCommandLineToolOptions(INamedTypeSymbol type, Compilation compilation)
    {
        var commandLineToolOptionsType = compilation.GetTypeByMetadataName(CommandLineToolOptionsFullName);
        if (commandLineToolOptionsType is null)
        {
            return false;
        }

        var current = type.BaseType;
        while (current is not null)
        {
            if (SymbolEqualityComparer.Default.Equals(current, commandLineToolOptionsType))
            {
                return true;
            }

            current = current.BaseType;
        }

        return false;
    }

    /// <summary>
    /// Gets the tool name from the [CliTool] attribute on the type or its base types.
    /// </summary>
    private static string? GetToolName(INamedTypeSymbol type)
    {
        var current = type;
        while (current is not null)
        {
            foreach (var attribute in current.GetAttributes())
            {
                if (attribute.AttributeClass?.ToDisplayString() == CliToolAttributeFullName)
                {
                    // Get the first constructor argument (the tool name)
                    if (attribute.ConstructorArguments.Length > 0 &&
                        attribute.ConstructorArguments[0].Value is string toolName)
                    {
                        return toolName;
                    }
                }
            }

            current = current.BaseType;
        }

        return null;
    }

    /// <summary>
    /// Gets the subcommand parts from [CliSubCommand] attributes.
    /// Stub implementation - will be completed in Task 2.3.
    /// </summary>
    private static IReadOnlyList<string> GetSubCommandParts(INamedTypeSymbol type)
    {
        // Stub implementation - returns empty list
        // Full implementation in Task 2.3
        return Array.Empty<string>();
    }

    /// <summary>
    /// Gets the CLI properties from the type.
    /// Stub implementation - will be completed in Task 2.3.
    /// </summary>
    private static IReadOnlyList<CliPropertyInfo> GetCliProperties(INamedTypeSymbol type)
    {
        // Stub implementation - returns empty list
        // Full implementation in Task 2.3
        return Array.Empty<CliPropertyInfo>();
    }

    /// <summary>
    /// Generates the source code for the options class.
    /// Stub implementation - will be completed in Task 2.4.
    /// </summary>
    private static void GenerateCode(SourceProductionContext context, OptionsClassInfo info)
    {
        // Stub implementation - no code generation yet
        // Full implementation in Task 2.4
    }
}
