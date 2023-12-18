using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("demo", "byo-access-token")]
public record AzDemoByoAccessTokenOptions(
[property: CommandSwitch("--access-token")] string AccessToken,
[property: CommandSwitch("--subscription-id")] string SubscriptionId
) : AzOptions
{
    [CommandSwitch("--theme")]
    public string? Theme { get; set; }
}

