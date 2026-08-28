using ModularPipelines.Cmd.Models;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Cmd;

internal class Cmd : ICmd
{
    private readonly IPipelineContext _context;

    public Cmd(IPipelineContext context)
    {
        _context = context;
    }

    /// <inheritdoc />
    public virtual Task<CommandResult> RunAsync(
        string script,
        CommandExecutionOptions? executionOptions = null,
        CancellationToken cancellationToken = default)
    {
        return RunAsync(new CmdScriptOptions(script), executionOptions, cancellationToken);
    }

    /// <inheritdoc />
    public virtual Task<CommandResult> RunAsync(
        CmdScriptOptions options,
        CommandExecutionOptions? executionOptions = null,
        CancellationToken cancellationToken = default)
    {
        return _context.Shell.RunAsync(options, executionOptions, cancellationToken);
    }

    /// <inheritdoc />
    public virtual Task<CommandResult> RunFileAsync(
        string path,
        CommandExecutionOptions? executionOptions = null,
        CancellationToken cancellationToken = default)
    {
        return _context.Shell.RunAsync(
            new CommandLineToolOptions(path),
            executionOptions,
            cancellationToken);
    }
}
