using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("eks", "associate-access-policy")]
public record AwsEksAssociateAccessPolicyOptions(
[property: CliOption("--cluster-name")] string ClusterName,
[property: CliOption("--principal-arn")] string PrincipalArn,
[property: CliOption("--policy-arn")] string PolicyArn,
[property: CliOption("--access-scope")] string AccessScope
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}