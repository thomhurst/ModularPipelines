using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
public class AzCommunication
{
    public AzCommunication(
        AzCommunicationChat chat,
        AzCommunicationEmail email,
        AzCommunicationIdentity identity,
        AzCommunicationPhonenumber phonenumber,
        AzCommunicationPhonenumbers phonenumbers,
        AzCommunicationRooms rooms,
        AzCommunicationSms sms,
        AzCommunicationUserIdentity userIdentity,
        ICommand internalCommand
    )
    {
        Chat = chat;
        Email = email;
        Identity = identity;
        Phonenumber = phonenumber;
        Phonenumbers = phonenumbers;
        Rooms = rooms;
        Sms = sms;
        UserIdentity = userIdentity;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzCommunicationChat Chat { get; }

    public AzCommunicationEmail Email { get; }

    public AzCommunicationIdentity Identity { get; }

    public AzCommunicationPhonenumber Phonenumber { get; }

    public AzCommunicationPhonenumbers Phonenumbers { get; }

    public AzCommunicationRooms Rooms { get; }

    public AzCommunicationSms Sms { get; }

    public AzCommunicationUserIdentity UserIdentity { get; }

    public async Task<CommandResult> Create(AzCommunicationCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzCommunicationDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzCommunicationDeleteOptions(), token);
    }

    public async Task<CommandResult> LinkNotificationHub(AzCommunicationLinkNotificationHubOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzCommunicationLinkNotificationHubOptions(), token);
    }

    public async Task<CommandResult> List(AzCommunicationListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzCommunicationListOptions(), token);
    }

    public async Task<CommandResult> ListKey(AzCommunicationListKeyOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzCommunicationListKeyOptions(), token);
    }

    public async Task<CommandResult> RegenerateKey(AzCommunicationRegenerateKeyOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzCommunicationRegenerateKeyOptions(), token);
    }

    public async Task<CommandResult> Show(AzCommunicationShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzCommunicationShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzCommunicationUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzCommunicationUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzCommunicationWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzCommunicationWaitOptions(), token);
    }
}