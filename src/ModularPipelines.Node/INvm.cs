using ModularPipelines.Models;

namespace ModularPipelines.Node;

public interface INvm
{
    Task<CommandResult> Use(string version, CancellationToken cancellationToken = default);
    
    Task<CommandResult> Install(string version, CancellationToken cancellationToken = default);
    
    Task<CommandResult> Version(CancellationToken cancellationToken = default);
    
    Task<CommandResult> Which(CancellationToken cancellationToken = default);
}
