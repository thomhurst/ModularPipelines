using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("guardduty", "get-filter")]
public record AwsGuarddutyGetFilterOptions(
[property: CliOption("--detector-id")] string DetectorId,
[property: CliOption("--filter-name")] string FilterName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}