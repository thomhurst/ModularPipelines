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
        var candidates = new List<(ExpressionSyntax Original, ExpressionSyntax Replacement)>();
        var member = node.FirstAncestorOrSelf<MemberAccessExpressionSyntax>();
        if (member is not null && model.GetSymbolInfo(member.Expression, context.CancellationToken).Symbol is INamedTypeSymbol
            { Name: "ModuleStatus", TypeKind: TypeKind.Enum } statusType && statusType.ContainingNamespace.ToDisplayString() == "ModularPipelines")
        {
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
            if (newName is not null)
            {
                candidates.Add((member, member.WithName(Rename(member.Name, newName))));
            }
        }

        foreach (var invocation in node.AncestorsAndSelf().OfType<InvocationExpressionSyntax>())
        {
            if (invocation.Expression is not MemberAccessExpressionSyntax access || invocation.ContainsDirectives)
            {
                continue;
            }

            var receiver = model.GetTypeInfo(access.Expression, context.CancellationToken).Type;
            if (receiver is null || receiver.TypeKind == TypeKind.Error)
            {
                continue;
            }

            if (invocation.ArgumentList.Arguments.Count == 0 && IsModuleContext(receiver))
            {
                var tools = SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression,
                    access.Expression, SyntaxFactory.IdentifierName("Tools"));
                candidates.Add((invocation, access.WithExpression(tools).WithTrailingTrivia(
                    access.GetTrailingTrivia().AddRange(invocation.ArgumentList.DescendantTrivia()))));
            }

            var arguments = invocation.ArgumentList.WithArguments(SyntaxFactory.SeparatedList(
                invocation.ArgumentList.Arguments.Select(argument =>
                    argument.NameColon?.Name.Identifier.ValueText == "token"
                        ? argument.WithNameColon(argument.NameColon.WithName(
                            SyntaxFactory.IdentifierName("cancellationToken").WithTriviaFrom(argument.NameColon.Name)))
                        : argument), invocation.ArgumentList.Arguments.GetSeparators()));
            var methodNames = access.Name.Identifier.ValueText.EndsWith("Async", StringComparison.Ordinal)
                ? new[] { access.Name.Identifier.ValueText }
                : new[] { access.Name.Identifier.ValueText, access.Name.Identifier.ValueText + "Async" };
            foreach (var name in methodNames)
            {
                var renamed = invocation.WithExpression(access.WithName(Rename(access.Name, name))).WithArgumentList(arguments);
                if (!renamed.IsEquivalentTo(invocation))
                {
                    candidates.Add((invocation, renamed));
                }

                if (receiver.ToDisplayString() != "ModularPipelines.Git.IGitCommands")
                {
                    continue;
                }

                foreach (var group in receiver.GetMembers().OfType<IPropertySymbol>().Where(property => !property.IsStatic))
                {
                    var grouped = SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression,
                        access.Expression, SyntaxFactory.IdentifierName(group.Name));
                    candidates.Add((invocation, renamed.WithExpression(access.WithExpression(grouped).WithName(Rename(access.Name, name)))));
                }
            }
        }

        var fixes = new List<(Document Document, ExpressionSyntax Replacement)>();
        foreach (var candidate in candidates)
        {
            var annotation = new SyntaxAnnotation();
            var changed = context.Document.WithSyntaxRoot(root.ReplaceNode(candidate.Original,
                candidate.Replacement.WithAdditionalAnnotations(annotation, Formatter.Annotation)));
            var changedRoot = await changed.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
            var changedModel = await changed.GetSemanticModelAsync(context.CancellationToken).ConfigureAwait(false);
            var replacement = changedRoot?.GetAnnotatedNodes(annotation).SingleOrDefault();
            if (replacement is null || changedModel is null)
            {
                continue;
            }

            var symbol = changedModel.GetSymbolInfo(replacement, context.CancellationToken).Symbol;
            if (symbol is not (IMethodSymbol or IPropertySymbol or IFieldSymbol) || !IsFrameworkSymbol(symbol)
                || changedModel.GetDiagnostics(replacement.Span, context.CancellationToken).Any(error => error.Severity == DiagnosticSeverity.Error))
            {
                continue;
            }

            if (!fixes.Any(fix => fix.Replacement.IsEquivalentTo(candidate.Replacement)))
            {
                fixes.Add((changed, candidate.Replacement));
            }
        }

        // More than one viable Git group or overload migration needs a human choice.
        if (fixes.Count == 1)
        {
            context.RegisterCodeFix(CodeAction.Create("Upgrade to ModularPipelines V4",
                _ => Task.FromResult(fixes[0].Document), nameof(V4UpgradeCodeFixProvider)), diagnostic);
        }
    }

    private static SimpleNameSyntax Rename(SimpleNameSyntax name, string replacement) => name switch
    {
        GenericNameSyntax generic => generic.WithIdentifier(SyntaxFactory.Identifier(replacement).WithTriviaFrom(generic.Identifier)),
        _ => SyntaxFactory.IdentifierName(replacement).WithTriviaFrom(name),
    };

    private static bool IsModuleContext(ITypeSymbol type) =>
        type.ToDisplayString() == "ModularPipelines.IModuleContext"
        || type.AllInterfaces.Any(@interface => @interface.ToDisplayString() == "ModularPipelines.IModuleContext");

    private static bool IsFrameworkSymbol(ISymbol symbol)
    {
        var name = symbol.ContainingNamespace.ToDisplayString();
        return name == "ModularPipelines" || name.StartsWith("ModularPipelines.", StringComparison.Ordinal);
    }
}
