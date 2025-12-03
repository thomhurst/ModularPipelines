using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloud-service", "role-instance", "show-instance-view")]
public record AzCloudServiceRoleInstanceShowInstanceViewOptions : AzOptions
{
    [CliOption("--cloud-service-name")]
    public string? CloudServiceName { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--role-instance-name")]
    public string? RoleInstanceName { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}