using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("emr-containers", "update-role-trust-policy")]
public record AwsEmrContainersUpdateRoleTrustPolicyOptions(
[property: CliOption("--cluster-name")] string ClusterName,
[property: CliOption("--namespace")] string Namespace,
[property: CliOption("--role-name")] string RoleName
) : AwsOptions
{
    [CliOption("--iam-endpoint")]
    public string? IamEndpoint { get; set; }

    [CliFlag("--dry-run")]
    public bool? DryRun { get; set; }
}