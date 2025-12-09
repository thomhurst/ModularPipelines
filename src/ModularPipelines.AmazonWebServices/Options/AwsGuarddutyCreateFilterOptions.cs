using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("guardduty", "create-filter")]
public record AwsGuarddutyCreateFilterOptions(
[property: CliOption("--detector-id")] string DetectorId,
[property: CliOption("--name")] string Name,
[property: CliOption("--finding-criteria")] string FindingCriteria
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--action")]
    public string? Action { get; set; }

    [CliOption("--rank")]
    public int? Rank { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}