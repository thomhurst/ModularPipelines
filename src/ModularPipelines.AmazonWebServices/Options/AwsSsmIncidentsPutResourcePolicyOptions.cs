using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ssm-incidents", "put-resource-policy")]
public record AwsSsmIncidentsPutResourcePolicyOptions(
[property: CliOption("--policy")] string Policy,
[property: CliOption("--resource-arn")] string ResourceArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}