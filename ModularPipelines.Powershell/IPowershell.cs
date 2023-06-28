using CliWrap.Buffered;
using ModularPipelines.Powershell.Models;

namespace ModularPipelines.Powershell;

public interface IPowershell
{
    Task<BufferedCommandResult> Script(PowershellScriptOptions options, CancellationToken cancellationToken = default);
    Task<BufferedCommandResult> FromFile(PowershellFileOptions options, CancellationToken cancellationToken = default);
}