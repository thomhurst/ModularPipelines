using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eventgrid", "partner")]
public class AzEventgridPartnerRegistration
{
    public AzEventgridPartnerRegistration(
        AzEventgridPartnerRegistrationCreate create,
        AzEventgridPartnerRegistrationDelete delete,
        AzEventgridPartnerRegistrationList list,
        AzEventgridPartnerRegistrationShow show,
        ICommand internalCommand
    )
    {
        CreateCommands = create;
        DeleteCommands = delete;
        ListCommands = list;
        ShowCommands = show;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzEventgridPartnerRegistrationCreate CreateCommands { get; }

    public AzEventgridPartnerRegistrationDelete DeleteCommands { get; }

    public AzEventgridPartnerRegistrationList ListCommands { get; }

    public AzEventgridPartnerRegistrationShow ShowCommands { get; }

    public async Task<CommandResult> Create(AzEventgridPartnerRegistrationCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzEventgridPartnerRegistrationDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzEventgridPartnerRegistrationDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzEventgridPartnerRegistrationListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzEventgridPartnerRegistrationListOptions(), token);
    }

    public async Task<CommandResult> Show(AzEventgridPartnerRegistrationShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzEventgridPartnerRegistrationShowOptions(), token);
    }
}