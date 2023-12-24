using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eks", "describe-access-entry")]
public record AwsEksDescribeAccessEntryOptions(
[property: CommandSwitch("--cluster-name")] string ClusterName,
[property: CommandSwitch("--principal-arn")] string PrincipalArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}