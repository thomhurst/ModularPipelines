using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("xray", "delete-resource-policy")]
public record AwsXrayDeleteResourcePolicyOptions(
[property: CliOption("--policy-name")] string PolicyName
) : AwsOptions
{
    [CliOption("--policy-revision-id")]
    public string? PolicyRevisionId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}