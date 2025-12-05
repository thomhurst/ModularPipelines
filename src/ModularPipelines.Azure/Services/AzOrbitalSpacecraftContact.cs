using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("orbital", "spacecraft")]
public class AzOrbitalSpacecraftContact
{
    public AzOrbitalSpacecraftContact(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzOrbitalSpacecraftContactCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzOrbitalSpacecraftContactDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzOrbitalSpacecraftContactDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzOrbitalSpacecraftContactListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzOrbitalSpacecraftContactShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzOrbitalSpacecraftContactShowOptions(), token);
    }

    public async Task<CommandResult> Wait(AzOrbitalSpacecraftContactWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzOrbitalSpacecraftContactWaitOptions(), token);
    }
}