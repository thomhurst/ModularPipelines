using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("ml")]
public class AzMlComputetarget
{
    public AzMlComputetarget(
        AzMlComputetargetAmlcompute amlcompute,
        AzMlComputetargetAttach attach,
        AzMlComputetargetComputeinstance computeinstance,
        AzMlComputetargetCreate create,
        AzMlComputetargetUpdate update,
        ICommand internalCommand
    )
    {
        Amlcompute = amlcompute;
        Attach = attach;
        Computeinstance = computeinstance;
        Create = create;
        Update = update;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzMlComputetargetAmlcompute Amlcompute { get; }

    public AzMlComputetargetAttach Attach { get; }

    public AzMlComputetargetComputeinstance Computeinstance { get; }

    public AzMlComputetargetCreate Create { get; }

    public AzMlComputetargetUpdate Update { get; }

    public async Task<CommandResult> Delete(AzMlComputetargetDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Detach(AzMlComputetargetDetachOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetCredentials(AzMlComputetargetGetCredentialsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzMlComputetargetListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMlComputetargetListOptions(), token);
    }

    public async Task<CommandResult> Show(AzMlComputetargetShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}