using System.Collections.Immutable;
using System.Composition;
using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Formatting;
using ModularPipelines.Analyzers.Extensions;

namespace ModularPipelines.Analyzers;

[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(ConsoleUseCodeFixProvider))]
[Shared]
[ExcludeFromCodeCoverage]
public sealed class ConsoleUseCodeFixProvider : CodeFixProvider
{
    private static readonly ImmutableHashSet<string> SupportedMethodNames =
        ImmutableHashSet.Create("Write", "WriteLine", "WriteAsync", "WriteLineAsync");

    /// <inheritdoc/>
    public override ImmutableArray<string> FixableDiagnosticIds =>
        ImmutableArray.Create(ConsoleUseAnalyzer.DiagnosticId);

    /// <inheritdoc/>
    public override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

    /// <inheritdoc/>
    public override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
        var semanticModel = await context.Document.GetSemanticModelAsync(context.CancellationToken).ConfigureAwait(false);
        var diagnostic = context.Diagnostics.First();
        var invocation = root?.FindNode(diagnostic.Location.SourceSpan)
            .FirstAncestorOrSelf<InvocationExpressionSyntax>();

        if (invocation is null
            || semanticModel?.GetSymbolInfo(invocation, context.CancellationToken).Symbol is not IMethodSymbol method
            || !SupportedMethodNames.Contains(method.Name)
            || !CanReplaceStatement(invocation)
            || !HasSupportedArguments(invocation, semanticModel, context.CancellationToken)
            || FindModuleContextParameter(invocation, semanticModel, context.CancellationToken) is not { } contextParameter)
        {
            return;
        }

        context.RegisterCodeFix(
            CodeAction.Create(
                CodeFixResources.ConsoleUseCodeFixTitle,
                cancellationToken => ReplaceWithLoggerAsync(
                    context.Document,
                    invocation,
                    contextParameter.Identifier.ValueText,
                    cancellationToken),
                nameof(CodeFixResources.ConsoleUseCodeFixTitle)),
            diagnostic);
    }

    private static bool CanReplaceStatement(InvocationExpressionSyntax invocation)
    {
        return invocation.Parent is ExpressionStatementSyntax
               || invocation.Parent is AwaitExpressionSyntax { Parent: ExpressionStatementSyntax };
    }

    private static bool HasSupportedArguments(
        InvocationExpressionSyntax invocation,
        SemanticModel semanticModel,
        CancellationToken cancellationToken)
    {
        var firstArgument = invocation.ArgumentList.Arguments.FirstOrDefault();
        return invocation.ArgumentList.Arguments.Count <= 1
               && (firstArgument is null
               || semanticModel.GetTypeInfo(firstArgument.Expression, cancellationToken).ConvertedType?.SpecialType
               == SpecialType.System_String);
    }

    private static ParameterSyntax? FindModuleContextParameter(
        InvocationExpressionSyntax invocation,
        SemanticModel semanticModel,
        CancellationToken cancellationToken)
    {
        var moduleContextType = semanticModel.Compilation.GetTypeByMetadataName(
            "ModularPipelines.Context.IModuleContext");
        var method = invocation.FirstAncestorOrSelf<MethodDeclarationSyntax>();

        return moduleContextType is null
               || method is null
               || invocation.Ancestors()
                   .TakeWhile(node => node != method)
                   .Any(IsStaticCallable)
            ? null
            : method.ParameterList.Parameters.FirstOrDefault(parameter =>
                SymbolEqualityComparer.Default.Equals(
                    semanticModel.GetTypeInfo(parameter.Type!, cancellationToken).Type,
                    moduleContextType));
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

    private static async Task<Document> ReplaceWithLoggerAsync(
        Document document,
        InvocationExpressionSyntax invocation,
        string contextParameterName,
        CancellationToken cancellationToken)
    {
        var root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
        if (root is null)
        {
            return document;
        }

        var logMethod = invocation.Expression.ToString().Contains("Console.Error.", StringComparison.Ordinal)
            ? "LogError"
            : "LogInformation";
        var arguments = invocation.ArgumentList.Arguments.Count == 0
            ? SyntaxFactory.SingletonSeparatedList(
                SyntaxFactory.Argument(
                    SyntaxFactory.LiteralExpression(
                        SyntaxKind.StringLiteralExpression,
                        SyntaxFactory.Literal(string.Empty))))
            : invocation.ArgumentList.Arguments;
        var loggerInvocation = SyntaxFactory.InvocationExpression(
                SyntaxFactory.ParseExpression($"{contextParameterName}.Logger.{logMethod}"),
                SyntaxFactory.ArgumentList(arguments))
            .WithAdditionalAnnotations(Formatter.Annotation);

        SyntaxNode oldNode;
        SyntaxNode newNode;

        if (invocation.Parent is AwaitExpressionSyntax awaitExpression
            && awaitExpression.Parent is ExpressionStatementSyntax awaitStatement)
        {
            oldNode = awaitStatement;
            newNode = SyntaxFactory.ExpressionStatement(loggerInvocation)
                .WithTriviaFrom(oldNode)
                .WithAdditionalAnnotations(Formatter.Annotation);
        }
        else
        {
            oldNode = invocation;
            newNode = loggerInvocation.WithTriviaFrom(invocation);
        }

        var newRoot = root.ReplaceNode(oldNode, newNode)
            .AddUsing("Microsoft.Extensions.Logging");
        return document.WithSyntaxRoot(newRoot);
    }
}
