using ModularPipelines.Context.Domains.Shell;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Context;

internal class Powershell : IPowerShellContext
{
    private readonly ICommandContext _command;

    public Powershell(ICommandContext command)
    {
        _command = command;
    }

    public virtual Task<CommandResult> RunAsync(
        string script,
        CommandExecutionOptions? executionOptions = null,
        CancellationToken cancellationToken = default)
    {
        return RunAsync(new PowershellScriptOptions(script), executionOptions, cancellationToken);
    }

    public virtual Task<CommandResult> RunAsync(
        PowershellScriptOptions options,
        CommandExecutionOptions? executionOptions = null,
        CancellationToken cancellationToken = default)
    {
        return _command.ExecuteCommandLineToolAsync(options, executionOptions, cancellationToken);
    }

    public virtual Task<CommandResult> RunFileAsync(
        string path,
        CommandExecutionOptions? executionOptions = null,
        CancellationToken cancellationToken = default)
    {
        return RunFileAsync(new PowershellFileOptions(path), executionOptions, cancellationToken);
    }

    public virtual Task<CommandResult> RunFileAsync(
        PowershellFileOptions options,
        CommandExecutionOptions? executionOptions = null,
        CancellationToken cancellationToken = default)
    {
        return _command.ExecuteCommandLineToolAsync(options, executionOptions, cancellationToken);
    }
}
