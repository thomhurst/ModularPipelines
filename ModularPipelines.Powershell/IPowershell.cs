using ModularPipelines.Models;
using ModularPipelines.Powershell.Models;

namespace ModularPipelines.Powershell;

public interface IPowershell
{
    Task<CommandResult> Script(PowershellScriptOptions options, CancellationToken cancellationToken = default);
    Task<CommandResult> FromFile(PowershellFileOptions options, CancellationToken cancellationToken = default);
}