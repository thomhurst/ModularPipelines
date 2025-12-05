using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("guardduty", "update-filter")]
public record AwsGuarddutyUpdateFilterOptions(
[property: CliOption("--detector-id")] string DetectorId,
[property: CliOption("--filter-name")] string FilterName
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--action")]
    public string? Action { get; set; }

    [CliOption("--rank")]
    public int? Rank { get; set; }

    [CliOption("--finding-criteria")]
    public string? FindingCriteria { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}