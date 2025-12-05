using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute-optimizer", "update-enrollment-status")]
public record AwsComputeOptimizerUpdateEnrollmentStatusOptions(
[property: CliOption("--status")] string Status
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}