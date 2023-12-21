using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("communication", "chat")]
public class AzCommunicationChatMessage
{
    public AzCommunicationChatMessage(
        AzCommunicationChatMessageReceipt receipt,
        ICommand internalCommand
    )
    {
        Receipt = receipt;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzCommunicationChatMessageReceipt Receipt { get; }

    public async Task<CommandResult> Delete(AzCommunicationChatMessageDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Get(AzCommunicationChatMessageGetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzCommunicationChatMessageListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Send(AzCommunicationChatMessageSendOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(AzCommunicationChatMessageUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}