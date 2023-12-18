using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
public class AzFleet
{
    public AzFleet(
        AzFleetMember member,
        AzFleetUpdaterun updaterun,
        AzFleetUpdatestrategy updatestrategy,
        ICommand internalCommand
    )
    {
        Member = member;
        Updaterun = updaterun;
        Updatestrategy = updatestrategy;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzFleetMember Member { get; }

    public AzFleetUpdaterun Updaterun { get; }

    public AzFleetUpdatestrategy Updatestrategy { get; }

    public async Task<CommandResult> Create(AzFleetCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzFleetDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetCredentials(AzFleetGetCredentialsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzFleetListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzFleetShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(AzFleetUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Wait(AzFleetWaitOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}