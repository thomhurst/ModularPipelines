using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Operations;

namespace ModularPipelines.Analyzers;

/// <summary>Proves the simple inline action path without guessing about mutable command state.</summary>
internal static class DirectCommandLineAction
{
    internal static IMethodSymbol? GetTarget(IInvocationOperation invocation)
    {
        var command = GetParsedCommand(invocation);
        if (command is null)
        {
            return null;
        }

        var statement = GetStatement(invocation);
        if (statement?.Parent is not IBlockOperation block
            || !HasStraightLinePrefix(block, statement))
        {
            return null;
        }

        var declaration = GetRootCommandDeclaration(block, command.Local);
        if (declaration is null)
        {
            return null;
        }

        var setter = GetOnlySetter(block, command, invocation.Syntax.SpanStart);
        if (setter is null || declaration.Syntax.SpanStart >= setter.Syntax.SpanStart)
        {
            return null;
        }

        return GetInlineTarget(setter);
    }

    private static ILocalReferenceOperation? GetParsedCommand(IInvocationOperation invocation)
    {
        if (invocation.TargetMethod.Name is not ("Invoke" or "InvokeAsync")
            || invocation.TargetMethod.ContainingType.ToDisplayString() != "System.CommandLine.ParseResult"
            || invocation.Instance is not IInvocationOperation parse
            || parse.TargetMethod.Name != "Parse"
            || parse.TargetMethod.ContainingType.ToDisplayString() != "System.CommandLine.Command"
            || parse.Instance is not ILocalReferenceOperation command)
        {
            return null;
        }

        return command;
    }

    private static IVariableDeclaratorOperation? GetRootCommandDeclaration(IBlockOperation block, ILocalSymbol command)
    {
        var declaration = block.Operations.SelectMany(static operation => operation.DescendantsAndSelf())
            .OfType<IVariableDeclaratorOperation>()
            .FirstOrDefault(candidate => SymbolEqualityComparer.Default.Equals(candidate.Symbol, command));
        if (declaration?.Initializer?.Value is not IObjectCreationOperation creation
            || creation.Type?.ToDisplayString() != "System.CommandLine.RootCommand")
        {
            return null;
        }

        return declaration;
    }

    private static IInvocationOperation? GetOnlySetter(
        IBlockOperation block,
        ILocalReferenceOperation command,
        int invocationStart)
    {
        IInvocationOperation? setter = null;
        foreach (var reference in block.DescendantsAndSelf().OfType<ILocalReferenceOperation>()
                     .Where(reference => SymbolEqualityComparer.Default.Equals(reference.Local, command.Local)))
        {
            if (ReferenceEquals(reference, command))
            {
                continue;
            }

            if (reference.Parent is not IInvocationOperation candidate
                || !ReferenceEquals(candidate.Instance, reference)
                || candidate.TargetMethod.Name != "SetAction"
                || candidate.TargetMethod.ContainingType.ToDisplayString() != "System.CommandLine.Command"
                || setter is not null
                || candidate.Parent is not IExpressionStatementOperation { Parent: IBlockOperation candidateBlock }
                || !ReferenceEquals(candidateBlock, block)
                || candidate.Syntax.SpanStart >= invocationStart)
            {
                return null;
            }

            setter = candidate;
        }

        return setter;
    }

    private static IMethodSymbol? GetInlineTarget(IInvocationOperation setter)
    {
        // Only the inline delegate itself is an entry point. Descendant lambdas are not invoked by SetAction.
        var value = setter.Arguments.FirstOrDefault()?.Value;
        if (value is IDelegateCreationOperation delegateCreation)
        {
            value = delegateCreation.Target;
        }

        if (value is not IAnonymousFunctionOperation action)
        {
            return null;
        }

        // Captures need execution-site reaching values, which this deliberately small proof does not infer.
        var captures = action.DescendantsAndSelf().Any(operation => operation switch
        {
            ILocalReferenceOperation local => !local.Local.DeclaringSyntaxReferences.All(
                syntax => action.Syntax.Span.Contains(syntax.Span)),
            IParameterReferenceOperation parameter => !SymbolEqualityComparer.Default.Equals(
                parameter.Parameter.ContainingSymbol, action.Symbol)
                && !parameter.Parameter.DeclaringSyntaxReferences.Any(
                    syntax => action.Syntax.Span.Contains(syntax.Span)),
            _ => false,
        });
        return captures ? null : action.Symbol;
    }

    private static bool HasStraightLinePrefix(IBlockOperation block, IOperation statement)
    {
        foreach (var operation in block.Operations)
        {
            if (ReferenceEquals(operation, statement))
            {
                return true;
            }

            if (operation is not (IVariableDeclarationGroupOperation or IExpressionStatementOperation))
            {
                return false;
            }
        }

        return false;
    }

    private static IOperation? GetStatement(IOperation operation)
    {
        while (true)
        {
            // Never cross a callable or control-flow boundary to manufacture a straight-line proof.
            if (operation is IAnonymousFunctionOperation or ILocalFunctionOperation
                or IConditionalOperation or ILoopOperation or ISwitchOperation
                or ISwitchExpressionOperation or IConditionalAccessOperation
                or ICoalesceOperation or ICoalesceAssignmentOperation
                or IBinaryOperation { OperatorKind: BinaryOperatorKind.ConditionalAnd or BinaryOperatorKind.ConditionalOr })
            {
                return null;
            }

            if (operation.Parent is null or IBlockOperation)
            {
                return operation;
            }

            operation = operation.Parent;
        }
    }
}
