using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Context;

/// <summary>
/// Provides functionality for executing PowerShell scripts and commands.
/// </summary>
/// <remarks>
/// This interface enables running PowerShell scripts either inline or from script files.
/// PowerShell execution is platform-dependent; on Windows it uses Windows PowerShell,
/// while on Linux/macOS it uses PowerShell Core (pwsh) if available.
/// For bash-specific execution, see <see cref="IBash"/>.
/// For general command execution, see <see cref="ICommand"/>.
/// </remarks>
public interface IPowershell
{
    /// <summary>
    /// Executes an inline PowerShell script.
    /// </summary>
    /// <param name="options">The script options containing the PowerShell script to execute and additional configuration.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A <see cref="CommandResult"/> containing the execution results, including standard output, standard error, and exit code.</returns>
    /// <example>
    /// <code>
    /// var result = await context.Powershell.Script(new PowershellScriptOptions("Get-Process | Select-Object -First 5"));
    /// Console.WriteLine(result.StandardOutput);
    /// </code>
    /// </example>
    Task<CommandResult> Script(PowershellScriptOptions options, CancellationToken cancellationToken = default);

    /// <summary>
    /// Executes a PowerShell script from a file.
    /// </summary>
    /// <param name="options">The file options containing the path to the PowerShell script file and additional configuration.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A <see cref="CommandResult"/> containing the execution results, including standard output, standard error, and exit code.</returns>
    /// <example>
    /// <code>
    /// var result = await context.Powershell.FromFile(new PowershellFileOptions("./scripts/deploy.ps1"));
    /// if (result.ExitCode != 0)
    /// {
    ///     throw new Exception($"Script failed: {result.StandardError}");
    /// }
    /// </code>
    /// </example>
    Task<CommandResult> FromFile(PowershellFileOptions options, CancellationToken cancellationToken = default);
}
