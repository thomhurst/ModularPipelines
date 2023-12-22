using ModularPipelines.Models;
using ModularPipelines.Node.Models;

namespace ModularPipelines.Node;

public interface INpx
{
    Task<CommandResult> ExecuteAsync(NpxOptions npxOptions, CancellationToken cancellationToken = default);
}