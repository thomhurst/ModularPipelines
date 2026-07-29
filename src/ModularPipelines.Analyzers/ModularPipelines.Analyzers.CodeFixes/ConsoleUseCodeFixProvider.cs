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
        ["WriteLine", "WriteLineAsync"];

    /// <inheritdoc/>
    public override ImmutableArray<string> FixableDiagnosticIds =>
        [ConsoleUseAnalyzer.DiagnosticId];

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
            || invocation.FindVisibleModuleContextParameter(
                semanticModel,
                context.CancellationToken) is not { } contextParameter)
        {
            return;
        }

        context.RegisterCodeFix(
            CodeAction.Create(
                CodeFixResources.ConsoleUseCodeFixTitle,
                cancellationToken => ReplaceWithLoggerAsync(
                    context.Document,
                    invocation,
                    contextParameter.Identifier,
                    IsConsoleError(invocation, semanticModel, context.CancellationToken),
                    MessageCanBeNull(invocation, semanticModel, context.CancellationToken),
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
        var arguments = invocation.ArgumentList.Arguments;
        if (invocation.ContainsDirectives)
        {
            return false;
        }

        if (arguments.Count != 1)
        {
            return arguments.Count == 0;
        }

        var argument = arguments[0];
        return argument.NameColon is null
               && semanticModel.GetTypeInfo(argument.Expression, cancellationToken).ConvertedType?.SpecialType
               == SpecialType.System_String;
    }

    private static bool IsConsoleError(
        InvocationExpressionSyntax invocation,
        SemanticModel semanticModel,
        CancellationToken cancellationToken)
    {
        if (invocation.Expression is not MemberAccessExpressionSyntax methodAccess
            || semanticModel.GetSymbolInfo(methodAccess.Expression, cancellationToken).Symbol
                is not IPropertySymbol property)
        {
            return false;
        }

        var consoleType = semanticModel.Compilation.GetTypeByMetadataName("System.Console");
        return property.Name == nameof(Console.Error)
               && SymbolEqualityComparer.Default.Equals(property.ContainingType, consoleType);
    }

    private static bool MessageCanBeNull(
        InvocationExpressionSyntax invocation,
        SemanticModel semanticModel,
        CancellationToken cancellationToken)
    {
        var arguments = invocation.ArgumentList.Arguments;
        return arguments.Count == 1
               && semanticModel.GetTypeInfo(arguments[0].Expression, cancellationToken)
                   .ConvertedNullability.FlowState != NullableFlowState.NotNull;
    }

    private static async Task<Document> ReplaceWithLoggerAsync(
        Document document,
        InvocationExpressionSyntax invocation,
        SyntaxToken contextParameterIdentifier,
        bool isConsoleError,
        bool messageCanBeNull,
        CancellationToken cancellationToken)
    {
        var root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
        if (root is null)
        {
            return document;
        }

        var logMethod = isConsoleError ? "LogError" : "LogInformation";
        var arguments = CreateLoggerArguments(
            invocation.ArgumentList,
            messageCanBeNull);
        var contextLogger = SyntaxFactory.MemberAccessExpression(
            SyntaxKind.SimpleMemberAccessExpression,
            SyntaxFactory.IdentifierName(contextParameterIdentifier.WithoutTrivia()),
            SyntaxFactory.IdentifierName("Logger"));
        var loggerInvocation = SyntaxFactory.InvocationExpression(
                SyntaxFactory.MemberAccessExpression(
                    SyntaxKind.SimpleMemberAccessExpression,
                    contextLogger,
                    SyntaxFactory.IdentifierName(logMethod)),
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

    private static SeparatedSyntaxList<ArgumentSyntax> CreateLoggerArguments(
        ArgumentListSyntax argumentList,
        bool messageCanBeNull)
    {
        var arguments = argumentList.Arguments;
        if (arguments.Count == 0)
        {
            return SyntaxFactory.SingletonSeparatedList(
                SyntaxFactory.Argument(
                    SyntaxFactory.LiteralExpression(
                        SyntaxKind.StringLiteralExpression,
                        SyntaxFactory.Literal(string.Empty)))
                    .WithLeadingTrivia(argumentList.OpenParenToken.TrailingTrivia));
        }

        var messageTrivia = argumentList.OpenParenToken.TrailingTrivia.AddRange(
            arguments[0].GetLeadingTrivia());
        if (messageTrivia.Count == 0
            || !messageTrivia[0].IsKind(SyntaxKind.WhitespaceTrivia))
        {
            messageTrivia = messageTrivia.Insert(0, SyntaxFactory.Space);
        }

        var argument = arguments[0].WithLeadingTrivia(default(SyntaxTriviaList));
        if (messageCanBeNull)
        {
            var parenthesizedMessage = SyntaxFactory.ParenthesizedExpression(
                argument.Expression.WithoutTrivia());
            argument = argument.WithExpression(
                    SyntaxFactory.BinaryExpression(
                        SyntaxKind.CoalesceExpression,
                        parenthesizedMessage,
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.StringKeyword)),
                            SyntaxFactory.IdentifierName(nameof(string.Empty)))))
                .WithTriviaFrom(argument);
        }

        return SyntaxFactory.SeparatedList<ArgumentSyntax>(
        [
            SyntaxFactory.Argument(
                SyntaxFactory.LiteralExpression(
                    SyntaxKind.StringLiteralExpression,
                    SyntaxFactory.Literal("{Message}"))),
            SyntaxFactory.Token(SyntaxKind.CommaToken)
                .WithTrailingTrivia(messageTrivia),
            argument,
        ]);
    }
}
