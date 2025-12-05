using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("cloud-service", "role-instance", "reimage")]
public record AzCloudServiceRoleInstanceReimageOptions : AzOptions
{
    [CliOption("--cloud-service-name")]
    public string? CloudServiceName { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--role-instance-name")]
    public string? RoleInstanceName { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}