using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("cloud-service", "update-domain", "walk-update-domain")]
public record AzCloudServiceUpdateDomainWalkUpdateDomainOptions : AzOptions
{
    [CliOption("--cloud-service-name")]
    public string? CloudServiceName { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--update-domain")]
    public string? UpdateDomain { get; set; }
}