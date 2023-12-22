using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("webapp", "auth", "twitter", "update")]
public record AzWebappAuthTwitterUpdateOptions : AzOptions
{
    [CommandSwitch("--consumer-key")]
    public string? ConsumerKey { get; set; }

    [CommandSwitch("--consumer-secret")]
    public string? ConsumerSecret { get; set; }

    [CommandSwitch("--consumer-secret-setting-name")]
    public string? ConsumerSecretSettingName { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--slot")]
    public string? Slot { get; set; }

    [CommandSwitch("--subscription")]
    public new string? Subscription { get; set; }

    [BooleanCommandSwitch("--yes")]
    public bool? Yes { get; set; }
}