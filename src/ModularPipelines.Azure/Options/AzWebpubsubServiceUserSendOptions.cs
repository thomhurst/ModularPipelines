using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("webpubsub", "service", "user", "send")]
public record AzWebpubsubServiceUserSendOptions(
[property: CommandSwitch("--hub-name")] string HubName,
[property: CommandSwitch("--payload")] string Payload,
[property: CommandSwitch("--user-id")] string UserId
) : AzOptions
{
    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--subscription")]
    public new string? Subscription { get; set; }
}