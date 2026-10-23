using System.Runtime.InteropServices;

namespace ModularPipelines.Node;

internal class Nvm(IPipelineContext context) : INvm
{
    // Keep caller-supplied values out of the script. Bash receives them as positional arguments.
    private static readonly string UnixScript = """
        if [ -z "${NVM_DIR:-}" ]; then
            if [ -n "${XDG_CONFIG_HOME:-}" ]; then
                export NVM_DIR="$XDG_CONFIG_HOME/nvm"
            else
                export NVM_DIR="$HOME/.nvm"
            fi
        fi
        source "$NVM_DIR/nvm.sh" --no-use && nvm "$@"
        """.ReplaceLineEndings("\n");

    public virtual Task<CommandResult> UseAsync(string version, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(version);
        return RunAsync(["use", version], cancellationToken);
    }

    public virtual Task<CommandResult> InstallAsync(string version, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(version);
        return RunAsync(["install", version], cancellationToken);
    }

    public virtual Task<CommandResult> VersionAsync(CancellationToken cancellationToken = default)
    {
        return RunAsync([IsWindows ? "version" : "--version"], cancellationToken);
    }

    public virtual Task<CommandResult> CurrentAsync(CancellationToken cancellationToken = default)
    {
        return RunAsync(["current"], cancellationToken);
    }

    private bool IsWindows => context.Environment.OperatingSystem == OSPlatform.Windows;

    private Task<CommandResult> RunAsync(IReadOnlyList<string> arguments, CancellationToken cancellationToken)
    {
        return IsWindows
            ? context.Shell.RunAsync("nvm", arguments, cancellationToken)
            : context.Shell.RunAsync("bash", ["-c", UnixScript, "nvm", .. arguments], cancellationToken);
    }
}
