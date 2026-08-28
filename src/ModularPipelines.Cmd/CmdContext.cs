using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Cmd;

internal class CmdContext : ICmdContext
{
    private readonly IPipelineContext _context;

    public CmdContext(IPipelineContext context)
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
        return RunFileAsync(new CmdFileOptions(path), executionOptions, cancellationToken);
    }

    /// <inheritdoc />
    public virtual Task<CommandResult> RunFileAsync(
        CmdFileOptions options,
        CommandExecutionOptions? executionOptions = null,
        CancellationToken cancellationToken = default)
    {
        return _context.Shell.RunAsync(options, executionOptions, cancellationToken);
    }
}
