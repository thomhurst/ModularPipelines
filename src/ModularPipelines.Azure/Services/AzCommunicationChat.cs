using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("communication")]
public class AzCommunicationChat
{
    public AzCommunicationChat(
        AzCommunicationChatMessage message,
        AzCommunicationChatParticipant participant,
        AzCommunicationChatThread thread
    )
    {
        Message = message;
        Participant = participant;
        Thread = thread;
    }

    public AzCommunicationChatMessage Message { get; }

    public AzCommunicationChatParticipant Participant { get; }

    public AzCommunicationChatThread Thread { get; }
}