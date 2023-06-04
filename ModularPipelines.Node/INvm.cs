using CliWrap.Buffered;

namespace ModularPipelines.Node;

public interface INvm<T> : INvm
{
}

public interface INvm
{
    Task<BufferedCommandResult> Use(string version, CancellationToken cancellationToken = default);
}