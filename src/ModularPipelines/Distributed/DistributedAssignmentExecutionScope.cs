namespace ModularPipelines.Distributed;

internal static class DistributedAssignmentExecutionScope
{
    private static readonly AsyncLocal<int> ScopeDepth = new();

    public static bool IsActive => ScopeDepth.Value > 0;

    public static IDisposable Enter()
    {
        ScopeDepth.Value++;
        return new Scope();
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
