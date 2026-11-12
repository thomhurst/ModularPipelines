using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Operations;

namespace ModularPipelines.Analyzers;

internal static partial class ModuleAuthoringAnalysis
{
    private static bool IsFrameworkPayloadAwaitableType(ITypeSymbol? type) =>
        type is INamedTypeSymbol { TypeArguments.Length: 1 } named
        && named.OriginalDefinition.ToDisplayString() is
            "System.Threading.Tasks.Task<TResult>" or "System.Threading.Tasks.ValueTask<TResult>"
            or "System.Runtime.CompilerServices.ConfiguredTaskAwaitable<TResult>"
            or "System.Runtime.CompilerServices.ConfiguredValueTaskAwaitable<TResult>";

    // Only framework wrappers preserve payload identity. In particular, an arbitrary
    // Result property, GetAwaiter implementation or conversion is not a proof.
    internal static IOperation? GetCancellationPayloadInput(IOperation operation) => operation switch
    {
        // Unwrap the value; the recursive proof must still reach the original token.
        IAwaitOperation awaited when IsFrameworkPayloadAwaitableType(awaited.Operation.Type) => awaited.Operation,
        IInvocationOperation invocation => GetFrameworkPayloadInvocationInput(invocation),
        IObjectCreationOperation { Arguments.Length: 1 } creation
            when creation.Type is INamedTypeSymbol named
            && named.OriginalDefinition.ToDisplayString() == "System.Threading.Tasks.ValueTask<TResult>"
            => creation.Arguments[0].Value,
        IPropertyReferenceOperation { Property.Name: "Result", Instance: { } instance } property
            when IsFrameworkPayloadContainer(property.Property.ContainingType) => instance,
        _ => null,
    };

    private static bool IsFrameworkPayloadContainer(INamedTypeSymbol type) =>
        type.OriginalDefinition.ToDisplayString() is
            "System.Threading.Tasks.Task<TResult>" or "System.Threading.Tasks.ValueTask<TResult>";

    private static IOperation? GetFrameworkPayloadInvocationInput(IInvocationOperation invocation) => invocation switch
    {
        { Arguments.Length: 1, TargetMethod: { Name: "FromResult", IsStatic: true } } factory
            when factory.TargetMethod.ContainingType.ToDisplayString() is
                "System.Threading.Tasks.Task" or "System.Threading.Tasks.ValueTask" => factory.Arguments[0].Value,
        { Instance: { } instance } wrapper
            when wrapper.TargetMethod.Name is "ConfigureAwait" or "AsTask"
            && IsFrameworkPayloadContainer(wrapper.TargetMethod.ContainingType) => instance,
        _ => null,
    };
}
