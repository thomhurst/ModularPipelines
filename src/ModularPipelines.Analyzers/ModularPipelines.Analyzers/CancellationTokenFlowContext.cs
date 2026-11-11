using Microsoft.CodeAnalysis;

namespace ModularPipelines.Analyzers;

internal sealed class CancellationTokenFlowContext
{
    private readonly HashSet<IMethodSymbol> _visitedMethods;
    private readonly Dictionary<IParameterSymbol, Dictionary<IOperation, bool>> _results =
        [with(SymbolEqualityComparer.Default)];

    public CancellationTokenFlowContext()
        : this([with(SymbolEqualityComparer.Default)])
    {
    }

    private CancellationTokenFlowContext(HashSet<IMethodSymbol> visitedMethods)
    {
        _visitedMethods = visitedMethods;
    }

    public bool TryGetResult(IOperation operation, IParameterSymbol sourceToken, out bool result)
    {
        result = false;
        return _results.TryGetValue(sourceToken, out var results)
            && results.TryGetValue(operation, out result);
    }

    public void RecordResult(IOperation operation, IParameterSymbol sourceToken, bool result)
    {
        if (!_results.TryGetValue(sourceToken, out var results))
        {
            results = [];
            _results.Add(sourceToken, results);
        }

        results[operation] = result;
    }

    public CancellationTokenFlowContext? VisitMethod(IMethodSymbol method)
    {
        var visitedMethods = new HashSet<IMethodSymbol>(_visitedMethods, SymbolEqualityComparer.Default);
        // Each method recursion context owns its cache: a recursion guard can change a result.
        return visitedMethods.Add(method) ? new CancellationTokenFlowContext(visitedMethods) : null;
    }
}
