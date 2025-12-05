using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("communication", "sms", "send")]
public record AzCommunicationSmsSendOptions(
[property: CliOption("--message")] string Message,
[property: CliOption("--recipient")] string Recipient,
[property: CliOption("--sender")] string Sender
) : AzOptions
{
    [CliOption("--connection-string")]
    public string? ConnectionString { get; set; }
}