using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eventgrid", "partner")]
public class AzEventgridPartnerDestination
{
    public AzEventgridPartnerDestination(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Activate(AzEventgridPartnerDestinationActivateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzEventgridPartnerDestinationActivateOptions(), token);
    }

    public async Task<CommandResult> Create(AzEventgridPartnerDestinationCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzEventgridPartnerDestinationDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzEventgridPartnerDestinationDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzEventgridPartnerDestinationListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzEventgridPartnerDestinationListOptions(), token);
    }

    public async Task<CommandResult> Show(AzEventgridPartnerDestinationShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzEventgridPartnerDestinationShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzEventgridPartnerDestinationUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzEventgridPartnerDestinationUpdateOptions(), token);
    }
}