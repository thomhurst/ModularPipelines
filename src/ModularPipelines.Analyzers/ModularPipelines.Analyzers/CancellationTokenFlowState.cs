using Microsoft.CodeAnalysis;

namespace ModularPipelines.Analyzers;

internal sealed class CancellationTokenFlowState
{
    public HashSet<IOperation> VisitedReferences { get; private set; } = [];

    // Only successful proofs are reusable: a failed proof can depend on the active
    // recursive helper path. Clones share successes but keep their own cycle guards.
    public HashSet<(IOperation Value, IParameterSymbol Token)> FlowingValues { get; private set; } = [];

    public CancellationTokenFlowState Clone() => new()
    {
        VisitedReferences = [with(VisitedReferences)],
        FlowingValues = FlowingValues,
    };
}
