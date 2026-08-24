using ModularPipelines.Exceptions;

namespace ModularPipelines.Engine;

internal sealed class PipelineExecutionState
{
    private int _state;

    public IDisposable EnterGraphExport()
    {
        while (true)
        {
            var state = Volatile.Read(ref _state);
            if (state < 0)
            {
                throw new PipelineException(
                    "The dependency graph must be exported before RunAsync starts.");
            }

            if (Interlocked.CompareExchange(ref _state, state + 1, state) == state)
            {
                return new GraphExportLease(this);
            }
        }
    }

    public void MarkExecutionStarted()
    {
        var state = Interlocked.CompareExchange(ref _state, -1, 0);
        if (state > 0)
        {
            throw new PipelineException(
                "RunAsync cannot start while a dependency graph export is in progress.");
        }
    }

    private sealed class GraphExportLease(PipelineExecutionState owner) : IDisposable
    {
        private PipelineExecutionState? _owner = owner;

        public void Dispose()
        {
            var currentOwner = Interlocked.Exchange(ref _owner, null);
            if (currentOwner is not null)
            {
                Interlocked.Decrement(ref currentOwner._state);
            }
        }
    }
}
