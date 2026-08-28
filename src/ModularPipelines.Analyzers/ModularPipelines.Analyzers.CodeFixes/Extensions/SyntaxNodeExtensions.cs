using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ModularPipelines.Analyzers.Extensions;

[ExcludeFromCodeCoverage]
internal static class SyntaxNodeExtensions
{
    public static SyntaxNode AddUsings(this SyntaxNode documentRoot)
    {
        return documentRoot.AddUsing("ModularPipelines.Attributes");
    }

    public static SyntaxNode AddUsing(this SyntaxNode documentRoot, string namespaceName)
    {
        var compilationUnitSyntax = (CompilationUnitSyntax) documentRoot;

        if (compilationUnitSyntax.Usings.Any(x =>
                x.Alias is null
                && !x.ContainsConditionalDirectives()
                && x.Name?.ToFullString() == namespaceName))
        {
            return documentRoot;
        }

        var endOfLine = compilationUnitSyntax
            .DescendantTrivia()
            .FirstOrDefault(trivia => trivia.IsKind(SyntaxKind.EndOfLineTrivia))
            .ToFullString();
        if (string.IsNullOrEmpty(endOfLine))
        {
            endOfLine = Environment.NewLine;
        }

        var usingDirective = SyntaxFactory
            .UsingDirective(SyntaxFactory.ParseName(namespaceName))
            .WithTrailingTrivia(SyntaxFactory.EndOfLine(endOfLine));
        var conditionalMatch = compilationUnitSyntax.Usings
            .FirstOrDefault(item =>
                item.Alias is null
                && item.Name?.ToFullString() == namespaceName);
        compilationUnitSyntax = conditionalMatch is null
            ? compilationUnitSyntax.AddUsings(usingDirective)
            : compilationUnitSyntax.WithUsings(
                compilationUnitSyntax.Usings.Insert(
                    compilationUnitSyntax.Usings.IndexOf(conditionalMatch),
                    usingDirective));

        return compilationUnitSyntax;
    }

    public static bool ContainsConditionalDirectives(this SyntaxNode node)
    {
        return node.DescendantTrivia(descendIntoTrivia: true)
            .Any(static trivia => trivia.Kind() is
                SyntaxKind.IfDirectiveTrivia
                or SyntaxKind.ElifDirectiveTrivia
                or SyntaxKind.ElseDirectiveTrivia
                or SyntaxKind.EndIfDirectiveTrivia);
    }

    public static ParameterSyntax? FindVisibleModuleContextParameter(
        this SyntaxNode node,
        SemanticModel semanticModel,
        CancellationToken cancellationToken)
    {
        var moduleContextType = semanticModel.Compilation.GetTypeByMetadataName(
            "ModularPipelines.IModuleContext");
        var method = node.FirstAncestorOrSelf<MethodDeclarationSyntax>();
        if (moduleContextType is null
            || method is null
            || node.Ancestors()
                .TakeWhile(ancestor => ancestor != method)
                .Any(IsStaticCallable))
        {
            return null;
        }

        var parameter = method.ParameterList.Parameters.FirstOrDefault(candidate =>
            SymbolEqualityComparer.Default.Equals(
                semanticModel.GetTypeInfo(candidate.Type!, cancellationToken).Type,
                moduleContextType));
        if (parameter is null
            || semanticModel.GetDeclaredSymbol(parameter, cancellationToken) is not { } parameterSymbol)
        {
            return null;
        }

        var visibleSymbol = semanticModel.GetSpeculativeSymbolInfo(
            node.SpanStart,
            SyntaxFactory.IdentifierName(parameter.Identifier.WithoutTrivia()),
            SpeculativeBindingOption.BindAsExpression).Symbol;
        return SymbolEqualityComparer.Default.Equals(visibleSymbol, parameterSymbol)
            ? parameter
            : null;
    }

    private static bool IsStaticCallable(SyntaxNode node)
    {
        return node switch
        {
            LocalFunctionStatementSyntax localFunction =>
                localFunction.Modifiers.Any(SyntaxKind.StaticKeyword),
            AnonymousFunctionExpressionSyntax anonymousFunction =>
                anonymousFunction.Modifiers.Any(SyntaxKind.StaticKeyword),
            _ => false,
        };
    }
}
