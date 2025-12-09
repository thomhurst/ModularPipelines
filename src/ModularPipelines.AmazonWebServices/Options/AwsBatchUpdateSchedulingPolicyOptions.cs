using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("batch", "update-scheduling-policy")]
public record AwsBatchUpdateSchedulingPolicyOptions(
[property: CliOption("--arn")] string Arn
) : AwsOptions
{
    [CliOption("--fairshare-policy")]
    public string? FairsharePolicy { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}