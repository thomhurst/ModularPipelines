using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloud-service", "update-domain", "show-update-domain")]
public record AzCloudServiceUpdateDomainShowUpdateDomainOptions : AzOptions
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