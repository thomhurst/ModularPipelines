using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ams", "live-event", "update")]
public record AzAmsLiveEventUpdateOptions : AzOptions
{
    [CliOption("--account-name")]
    public int? AccountName { get; set; }

    [CliOption("--add")]
    public string? Add { get; set; }

    [CliOption("--client-access-policy")]
    public string? ClientAccessPolicy { get; set; }

    [CliOption("--cross-domain-policy")]
    public string? CrossDomainPolicy { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--ips")]
    public string? Ips { get; set; }

    [CliOption("--key-frame-interval-duration")]
    public string? KeyFrameIntervalDuration { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--preview-ips")]
    public string? PreviewIps { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}