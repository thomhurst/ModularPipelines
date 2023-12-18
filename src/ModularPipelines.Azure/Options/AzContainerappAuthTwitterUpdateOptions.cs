using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("containerapp", "auth", "twitter", "update")]
public record AzContainerappAuthTwitterUpdateOptions : AzOptions
{
    [CommandSwitch("--consumer-key")]
    public string? ConsumerKey { get; set; }

    [CommandSwitch("--consumer-secret")]
    public string? ConsumerSecret { get; set; }

    [CommandSwitch("--consumer-secret-name")]
    public string? ConsumerSecretName { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }

    [BooleanCommandSwitch("--yes")]
    public bool? Yes { get; set; }
}