using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Context;

public interface IPowershell
{
    Task<CommandResult> Script(PowershellScriptOptions options, CancellationToken cancellationToken = default);

    Task<CommandResult> FromFile(PowershellFileOptions options, CancellationToken cancellationToken = default);
}