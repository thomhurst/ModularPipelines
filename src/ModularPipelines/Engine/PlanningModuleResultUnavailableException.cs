namespace ModularPipelines.Engine;

internal sealed class PlanningModuleResultUnavailableException : InvalidOperationException
{
    public PlanningModuleResultUnavailableException(Type moduleType)
        : base($"The result of module '{moduleType.Name}' is unavailable while building a pipeline plan.")
    {
    }
}

internal static class PlanningModuleResultAccess
{
    private static readonly AsyncLocal<int> ScopeDepth = new();

    public static IDisposable Enter()
    {
        ScopeDepth.Value++;
        return new Scope();
    }

    public static void ThrowIfUnavailable(Type moduleType)
    {
        if (ScopeDepth.Value > 0)
        {
            throw new PlanningModuleResultUnavailableException(moduleType);
        }
    }

    private sealed class Scope : IDisposable
    {
        private bool _disposed;

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            ScopeDepth.Value--;
            _disposed = true;
        }
    }
}
