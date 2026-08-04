using Microsoft.Extensions.FileProviders;

namespace ModularPipelines;

internal sealed class PipelineBuilderResources(string contentRootPath) : IDisposable
{
    private readonly object _lock = new();
    private IFileProvider? _contentRootFileProvider;
    private bool _disposed;

    public IFileProvider ContentRootFileProvider
    {
        get
        {
            lock (_lock)
            {
                ObjectDisposedException.ThrowIf(_disposed, this);
                return _contentRootFileProvider ??= new PhysicalFileProvider(contentRootPath);
            }
        }

        set
        {
            ArgumentNullException.ThrowIfNull(value);

            IFileProvider? previousProvider;
            lock (_lock)
            {
                ObjectDisposedException.ThrowIf(_disposed, this);
                if (ReferenceEquals(_contentRootFileProvider, value))
                {
                    return;
                }

                previousProvider = _contentRootFileProvider;
                _contentRootFileProvider = value;
            }

            (previousProvider as IDisposable)?.Dispose();
        }
    }

    ~PipelineBuilderResources()
    {
        Dispose(disposing: false);
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
        IFileProvider? provider;
        lock (_lock)
        {
            if (_disposed)
            {
                return;
            }

            _disposed = true;
            provider = _contentRootFileProvider;
            _contentRootFileProvider = null;
        }

        try
        {
            (provider as IDisposable)?.Dispose();
        }
        catch when (!disposing)
        {
            // Finalizers must not surface cleanup failures.
        }
    }
}
