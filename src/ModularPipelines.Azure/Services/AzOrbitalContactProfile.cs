using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("orbital")]
public class AzOrbitalContactProfile
{
    public AzOrbitalContactProfile(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzOrbitalContactProfileCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzOrbitalContactProfileDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzOrbitalContactProfileDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzOrbitalContactProfileListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzOrbitalContactProfileListOptions(), token);
    }

    public async Task<CommandResult> Show(AzOrbitalContactProfileShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzOrbitalContactProfileShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzOrbitalContactProfileUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzOrbitalContactProfileUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzOrbitalContactProfileWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzOrbitalContactProfileWaitOptions(), token);
    }
}