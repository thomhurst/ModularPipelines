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

        if (compilationUnitSyntax.Usings.Any(x => x.Name?.ToFullString() == namespaceName))
        {
            return documentRoot;
        }

        compilationUnitSyntax = compilationUnitSyntax.AddUsings(
            SyntaxFactory.UsingDirective(SyntaxFactory.ParseName(namespaceName)));

        return compilationUnitSyntax;
    }
}
