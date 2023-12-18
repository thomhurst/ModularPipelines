using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("communication", "sms", "send")]
public record AzCommunicationSmsSendOptions(
[property: CommandSwitch("--message")] string Message,
[property: CommandSwitch("--recipient")] string Recipient,
[property: CommandSwitch("--sender")] string Sender
) : AzOptions
{
    [CommandSwitch("--connection-string")]
    public string? ConnectionString { get; set; }
}