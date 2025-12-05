using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("logs", "put-resource-policy")]
public record AwsLogsPutResourcePolicyOptions : AwsOptions
{
    [CliOption("--policy-name")]
    public string? PolicyName { get; set; }

    [CliOption("--policy-document")]
    public string? PolicyDocument { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}