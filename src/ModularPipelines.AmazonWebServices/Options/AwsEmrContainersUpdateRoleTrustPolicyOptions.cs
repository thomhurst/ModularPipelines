using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("emr-containers", "update-role-trust-policy")]
public record AwsEmrContainersUpdateRoleTrustPolicyOptions(
[property: CommandSwitch("--cluster-name")] string ClusterName,
[property: CommandSwitch("--namespace")] string Namespace,
[property: CommandSwitch("--role-name")] string RoleName
) : AwsOptions
{
    [CommandSwitch("--iam-endpoint")]
    public string? IamEndpoint { get; set; }

    [BooleanCommandSwitch("--dry-run")]
    public bool? DryRun { get; set; }
}