using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("macie2", "update-findings-filter")]
public record AwsMacie2UpdateFindingsFilterOptions(
[property: CliOption("--id")] string Id
) : AwsOptions
{
    [CliOption("--action")]
    public string? Action { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--finding-criteria")]
    public string? FindingCriteria { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--position")]
    public int? Position { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}