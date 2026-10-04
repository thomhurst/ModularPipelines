using System.Collections.Immutable;
using System.Composition;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Formatting;

namespace ModularPipelines.Analyzers;

[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(V4UpgradeCodeFixProvider))]
[Shared]
public sealed class V4UpgradeCodeFixProvider : CodeFixProvider
{
    public override ImmutableArray<string> FixableDiagnosticIds => ["CS1061", "CS0117", "CS1739"];

    public override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

    public override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
        var model = await context.Document.GetSemanticModelAsync(context.CancellationToken).ConfigureAwait(false);
        if (root is null || model is null)
        {
            return;
        }

        var diagnostic = context.Diagnostics.First();
        var node = root.FindNode(diagnostic.Location.SourceSpan, getInnermostNodeForTie: true);
        var fixes = new List<(Document Document, ExpressionSyntax Replacement)>();
        foreach (var candidate in GetCandidates(node, model, context.CancellationToken))
        {
            if (fixes.Any(fix => fix.Replacement.IsEquivalentTo(candidate.Replacement)))
            {
                continue;
            }

            var changed = await BindReplacementAsync(context.Document, root, candidate.Original,
                candidate.Replacement, context.CancellationToken).ConfigureAwait(false);
            if (changed is not null)
            {
                fixes.Add((changed, candidate.Replacement));
            }

            // Different viable replacements, including different Git groups, need a human choice.
            if (fixes.Count > 1)
            {
                return;
            }
        }

        if (fixes.Count == 1)
        {
            context.RegisterCodeFix(CodeAction.Create("Upgrade to ModularPipelines V4",
                _ => Task.FromResult(fixes[0].Document), nameof(V4UpgradeCodeFixProvider)), diagnostic);
        }
    }

    private static IEnumerable<(ExpressionSyntax Original, ExpressionSyntax Replacement)> GetCandidates(
        SyntaxNode node, SemanticModel model, CancellationToken cancellationToken)
    {
        var member = node.FirstAncestorOrSelf<MemberAccessExpressionSyntax>();
        if (member is not null && GetStatusReplacement(member, model, cancellationToken) is { } statusReplacement)
        {
            yield return (member, statusReplacement);
        }

        foreach (var invocation in node.AncestorsAndSelf().OfType<InvocationExpressionSyntax>().Where(call => !call.ContainsDirectives))
        {
            foreach (var replacement in GetInvocationCandidates(invocation, model, cancellationToken))
            {
                yield return (invocation, replacement);
            }
        }
    }

    private static IEnumerable<ExpressionSyntax> GetInvocationCandidates(
        InvocationExpressionSyntax invocation, SemanticModel model, CancellationToken cancellationToken)
    {
        if (invocation.Expression is MemberBindingExpressionSyntax binding)
        {
            foreach (var replacement in GetConditionalInvocationReplacements(invocation, binding, model, cancellationToken))
            {
                yield return replacement;
            }

            yield break;
        }

        if (invocation.Expression is not MemberAccessExpressionSyntax access
            || model.GetTypeInfo(access.Expression, cancellationToken).Type is not { TypeKind: not TypeKind.Error } receiver)
        {
            yield break;
        }

        foreach (var replacement in GetInvocationReplacements(invocation, access, receiver))
        {
            yield return replacement;
        }
    }

    private static IEnumerable<ExpressionSyntax> GetConditionalInvocationReplacements(
        InvocationExpressionSyntax invocation, MemberBindingExpressionSyntax binding,
        SemanticModel model, CancellationToken cancellationToken)
    {
        var conditional = binding.Ancestors().OfType<ConditionalAccessExpressionSyntax>()
            .FirstOrDefault(access => access.WhenNotNull.Span.Contains(binding.Span));
        if (conditional is null
            || model.GetTypeInfo(conditional.Expression, cancellationToken).Type is not { TypeKind: not TypeKind.Error } receiver)
        {
            yield break;
        }

        // Reuse the ordinary member transformations, then restore the member binding
        // inside the existing conditional access. The receiver is never evaluated twice.
        var receiverAnnotation = new SyntaxAnnotation();
        var access = SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression,
            conditional.Expression.WithoutTrivia().WithAdditionalAnnotations(receiverAnnotation),
            binding.OperatorToken, binding.Name);
        foreach (var replacement in GetInvocationReplacements(invocation, access, receiver))
        {
            var annotatedReceiver = replacement.GetAnnotatedNodes(receiverAnnotation).Single();
            if (annotatedReceiver.Parent is MemberAccessExpressionSyntax leadingAccess)
            {
                yield return replacement.ReplaceNode(leadingAccess,
                    SyntaxFactory.MemberBindingExpression(leadingAccess.OperatorToken, leadingAccess.Name));
            }
        }
    }

    private static ExpressionSyntax? GetStatusReplacement(
        MemberAccessExpressionSyntax member, SemanticModel model, CancellationToken cancellationToken)
    {
        if (model.GetSymbolInfo(member.Expression, cancellationToken).Symbol is not INamedTypeSymbol
            { Name: "ModuleStatus", TypeKind: TypeKind.Enum } statusType
            || statusType.ContainingNamespace.ToDisplayString() != "ModularPipelines")
        {
            return null;
        }

        var newName = member.Name.Identifier.ValueText switch
        {
            "NotYetStarted" => "NotStarted",
            "Processing" => "Running",
            "Successful" => "Succeeded",
            "UsedHistory" => "RestoredFromHistory",
            "IgnoredFailure" => "FailureIgnored",
            "PipelineTerminated" or "Cancelled" => "Canceled",
            "CachedResult" => "RestoredFromCache",
            _ => null,
        };
        return newName is null ? null : member.WithName(Rename(member.Name, newName));
    }

    private static IEnumerable<ExpressionSyntax> GetInvocationReplacements(
        InvocationExpressionSyntax invocation, MemberAccessExpressionSyntax access, ITypeSymbol receiver)
    {
        if (invocation.ArgumentList.Arguments.Count == 0 && IsPipelineContext(receiver))
        {
            var tools = SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression,
                access.Expression, SyntaxFactory.IdentifierName("Tools"));
            yield return access.WithExpression(tools).WithTrailingTrivia(
                access.GetTrailingTrivia().AddRange(invocation.ArgumentList.DescendantTrivia()));
        }

        var arguments = RenameCancellationArgument(invocation.ArgumentList);
        var originalName = access.Name.Identifier.ValueText;
        var methodNames = originalName.EndsWith("Async", StringComparison.Ordinal)
            ? new[] { originalName }
            : new[] { originalName, originalName + "Async" };
        foreach (var name in methodNames)
        {
            var renamedAccess = access.WithName(Rename(access.Name, name));
            var renamed = invocation.WithExpression(renamedAccess).WithArgumentList(arguments);
            if (!renamed.IsEquivalentTo(invocation))
            {
                yield return renamed;
            }

            foreach (var group in GetGitGroupReceivers(receiver, access.Expression))
            {
                yield return renamed.WithExpression(renamedAccess.WithExpression(group));
            }
        }
    }

    private static ArgumentListSyntax RenameCancellationArgument(ArgumentListSyntax arguments) =>
        arguments.WithArguments(SyntaxFactory.SeparatedList(arguments.Arguments.Select(argument =>
            argument.NameColon?.Name.Identifier.ValueText == "token"
                ? argument.WithNameColon(argument.NameColon.WithName(
                    SyntaxFactory.IdentifierName("cancellationToken").WithTriviaFrom(argument.NameColon.Name)))
                : argument), arguments.Arguments.GetSeparators()));

    private static IEnumerable<ExpressionSyntax> GetGitGroupReceivers(ITypeSymbol receiver, ExpressionSyntax expression)
    {
        if (receiver.ToDisplayString() == "ModularPipelines.Git.IGit")
        {
            var commands = receiver.GetMembers("Commands").OfType<IPropertySymbol>().FirstOrDefault();
            if (commands is null)
            {
                yield break;
            }

            receiver = commands.Type;
            expression = SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression,
                expression, SyntaxFactory.IdentifierName(commands.Name));
        }

        if (receiver.ToDisplayString() != "ModularPipelines.Git.IGitCommands")
        {
            yield break;
        }

        foreach (var group in receiver.GetMembers().OfType<IPropertySymbol>().Where(property => !property.IsStatic))
        {
            yield return SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression,
                expression, SyntaxFactory.IdentifierName(group.Name));
        }
    }

    private static async Task<Document?> BindReplacementAsync(Document document, SyntaxNode root,
        ExpressionSyntax original, ExpressionSyntax replacement, CancellationToken cancellationToken)
    {
        var annotation = new SyntaxAnnotation();
        var changed = document.WithSyntaxRoot(root.ReplaceNode(original,
            replacement.WithAdditionalAnnotations(annotation, Formatter.Annotation)));
        var changedRoot = await changed.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
        var changedModel = await changed.GetSemanticModelAsync(cancellationToken).ConfigureAwait(false);
        var changedNode = changedRoot?.GetAnnotatedNodes(annotation).SingleOrDefault();
        if (changedNode is null || changedModel is null)
        {
            return null;
        }

        var symbol = changedModel.GetSymbolInfo(changedNode, cancellationToken).Symbol;
        if (symbol is not (IMethodSymbol or IPropertySymbol or IFieldSymbol) || !IsFrameworkSymbol(symbol)
            || changedModel.GetDiagnostics(changedNode.Span, cancellationToken).Any(error => error.Severity == DiagnosticSeverity.Error))
        {
            return null;
        }

        return changed;
    }

    private static SimpleNameSyntax Rename(SimpleNameSyntax name, string replacement) => name switch
    {
        GenericNameSyntax generic => generic.WithIdentifier(SyntaxFactory.Identifier(replacement).WithTriviaFrom(generic.Identifier)),
        _ => SyntaxFactory.IdentifierName(replacement).WithTriviaFrom(name),
    };

    private static bool IsPipelineContext(ITypeSymbol type) =>
        type.ToDisplayString() == "ModularPipelines.IPipelineContext"
        || type.AllInterfaces.Any(@interface => @interface.ToDisplayString() == "ModularPipelines.IPipelineContext");

    private static bool IsFrameworkSymbol(ISymbol symbol)
    {
        var name = symbol.ContainingNamespace.ToDisplayString();
        return name == "ModularPipelines" || name.StartsWith("ModularPipelines.", StringComparison.Ordinal);
    }
}
