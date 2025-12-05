using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("accessanalyzer", "start-policy-generation")]
public record AwsAccessanalyzerStartPolicyGenerationOptions(
[property: CliOption("--policy-generation-details")] string PolicyGenerationDetails
) : AwsOptions
{
    [CliOption("--cloud-trail-details")]
    public string? CloudTrailDetails { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}