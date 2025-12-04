using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("webapp", "auth", "twitter", "update")]
public record AzWebappAuthTwitterUpdateOptions : AzOptions
{
    [CliOption("--consumer-key")]
    public string? ConsumerKey { get; set; }

    [CliOption("--consumer-secret")]
    public string? ConsumerSecret { get; set; }

    [CliOption("--consumer-secret-setting-name")]
    public string? ConsumerSecretSettingName { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--slot")]
    public string? Slot { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}