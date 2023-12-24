using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("redshift", "delete-cluster-security-group")]
public record AwsRedshiftDeleteClusterSecurityGroupOptions(
[property: CommandSwitch("--cluster-security-group-name")] string ClusterSecurityGroupName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}