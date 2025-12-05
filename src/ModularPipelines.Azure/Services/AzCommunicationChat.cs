using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("communication")]
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