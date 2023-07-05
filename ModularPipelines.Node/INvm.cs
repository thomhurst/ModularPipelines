using ModularPipelines.Models;

namespace ModularPipelines.Node;

public interface INvm
{
    Task<CommandResult> Use(string version, CancellationToken cancellationToken = default);
}