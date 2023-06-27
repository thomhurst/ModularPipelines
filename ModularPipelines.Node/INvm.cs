using CliWrap.Buffered;

namespace ModularPipelines.Node;

public interface INvm
{
    Task<BufferedCommandResult> Use(string version, CancellationToken cancellationToken = default);
}