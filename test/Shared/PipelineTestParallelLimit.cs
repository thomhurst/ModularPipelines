[assembly: TUnit.Core.ParallelLimiter<ModularPipelines.TestHelpers.PipelineTestParallelLimit>]

namespace ModularPipelines.TestHelpers;

// These suites construct many independent pipelines. TUnit's ProcessorCount * 4
// default oversubscribes their CPU-bound startup under build/coverage load, so
// outer test deadlines can cancel initialization before the behavior is exercised.
// Bound cold startup as well as steady-state execution. Tests still exercise their
// own concurrent operations, with unchanged cancellation and timeout budgets.
public sealed class PipelineTestParallelLimit : TUnit.Core.Interfaces.IParallelLimit
{
    public int Limit => 2;
}
