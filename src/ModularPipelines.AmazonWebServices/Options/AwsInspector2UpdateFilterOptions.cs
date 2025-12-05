using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("inspector2", "update-filter")]
public record AwsInspector2UpdateFilterOptions(
[property: CliOption("--filter-arn")] string FilterArn
) : AwsOptions
{
    [CliOption("--action")]
    public string? Action { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--filter-criteria")]
    public string? FilterCriteria { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--reason")]
    public string? Reason { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}