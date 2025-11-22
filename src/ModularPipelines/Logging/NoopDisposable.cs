using System.Diagnostics.CodeAnalysis;

namespace ModularPipelines.Logging;

[ExcludeFromCodeCoverage]
internal class NoopDisposable : IDisposable
{
    public void Dispose()
    {
        // Stub class
    }
}
