using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ModularPipelines.SourceGenerator;

/// <summary>
/// Source generator that validates CommandLineToolOptions classes
/// and generates optimized BuildCommandLine() methods.
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

        // Get subcommand parts (stub - will be implemented in property extraction phase)
        var subCommandParts = GetSubCommandParts(typeSymbol);

        // Get CLI properties (stub - will be implemented in property extraction phase)
        var properties = GetCliProperties(typeSymbol);

        // Only process classes that have [CliTool] attribute OR CLI property attributes.
        // Legacy classes without these should be silently ignored.
        if (toolName is null && properties.Count == 0)
        {
            return null;
        }

        return new OptionsClassInfo(
            Namespace: namespaceName,
            ClassName: typeSymbol.Name,
            ToolName: toolName,
            SubCommandParts: subCommandParts,
            Properties: properties,
            IsPartial: isPartial,
            Location: typeDeclaration.Identifier.GetLocation(),
            BuildMethodModifier: GetBuildMethodModifier(typeSymbol, semanticModel.Compilation)
        );
    }

    /// <summary>
    /// Determines the modifier for the generated BuildCommandLine() method so that
    /// derived options classes override the base method instead of hiding it (CS0108).
    /// Base-most generated classes get "virtual "; classes whose ancestor also gets a
    /// generated method get "override "; sealed base-most classes get no modifier.
    /// </summary>
    private static string GetBuildMethodModifier(INamedTypeSymbol type, Compilation compilation)
    {
        for (var current = type.BaseType; current is not null; current = current.BaseType)
        {
            // Ancestor compiled in another assembly: its generated method is visible as metadata.
            var existing = current.GetMembers("BuildCommandLine")
                .OfType<IMethodSymbol>()
                .FirstOrDefault(m => m.Parameters.IsEmpty && !m.IsStatic);
            if (existing is not null)
            {
                return existing.IsVirtual || existing.IsOverride ? "override " : "new ";
            }

            // Ancestor in the current compilation: its generated method is not visible to us,
            // so predict whether GenerateCode will emit one for it.
            if (WouldGenerateBuildMethod(current, compilation))
            {
                return "override ";
            }
        }

        return type.IsSealed ? string.Empty : "virtual ";
    }

    /// <summary>
    /// Predicts whether GenerateCode will emit a BuildCommandLine() method for the given
    /// source-declared type, mirroring the checks in GetOptionsClassInfo and GenerateCode.
    /// </summary>
    private static bool WouldGenerateBuildMethod(INamedTypeSymbol type, Compilation compilation)
    {
        if (!InheritsFromCommandLineToolOptions(type, compilation))
        {
            return false;
        }

        if (GetToolName(type) is null && GetCliProperties(type).Count == 0)
        {
            return false;
        }

        return IsDeclaredPartial(type);
    }

    /// <summary>
    /// Checks whether any declaration of the type carries the partial keyword.
    /// GenerateCode skips non-partial types, so they never receive a generated method.
    /// </summary>
    private static bool IsDeclaredPartial(INamedTypeSymbol type)
    {
        foreach (var reference in type.DeclaringSyntaxReferences)
        {
            if (reference.GetSyntax() is TypeDeclarationSyntax declaration
                && declaration.Modifiers.Any(SyntaxKind.PartialKeyword))
            {
                return true;
            }
        }

        return false;
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
    /// Stub implementation - will be completed in property extraction phase.
    /// </summary>
    private static IReadOnlyList<string> GetSubCommandParts(INamedTypeSymbol type)
    {
        // Stub implementation - returns empty list
        // Full implementation will walk the type hierarchy to collect [CliSubCommand] attributes
        return Array.Empty<string>();
    }

    /// <summary>
    /// Gets the CLI properties from the type.
    /// Stub implementation - will be completed in property extraction phase.
    /// </summary>
    private static IReadOnlyList<CliPropertyInfo> GetCliProperties(INamedTypeSymbol type)
    {
        // Stub implementation - returns empty list
        // Full implementation will scan properties for [CliOption], [CliFlag], [CliArgument] attributes
        // and report MP0004 for properties without CLI attributes
        return Array.Empty<CliPropertyInfo>();
    }

    /// <summary>
    /// Generates the source code for the options class.
    /// Validates the class and reports diagnostics.
    /// </summary>
    private static void GenerateCode(SourceProductionContext context, OptionsClassInfo info)
    {
        // Validate partial keyword
        if (!info.IsPartial)
        {
            context.ReportDiagnostic(Diagnostic.Create(
                Diagnostics.MissingPartialKeyword,
                info.Location,
                info.ClassName));
            return; // Can't generate without partial
        }

        // Warn about missing [CliTool]
        if (info.ToolName is null)
        {
            context.ReportDiagnostic(Diagnostic.Create(
                Diagnostics.MissingCliToolAttribute,
                info.Location,
                info.ClassName));
        }

        // Check for duplicate argument positions
        var argumentPositions = info.Properties
            .Where(p => p.Kind == CliPropertyKind.Argument && p.ArgumentPosition.HasValue)
            .GroupBy(p => p.ArgumentPosition!.Value)
            .Where(g => g.Count() > 1);

        foreach (var group in argumentPositions)
        {
            var propNames = string.Join(", ", group.Select(p => p.Name));
            context.ReportDiagnostic(Diagnostic.Create(
                Diagnostics.DuplicateArgumentPosition,
                info.Location,
                propNames, group.Key));
        }

        // Generate BuildCommandLine() method
        var source = BuildMethodGenerator.Generate(info);
        context.AddSource($"{info.ClassName}.g.cs", source);
    }
}
