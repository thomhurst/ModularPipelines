using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("securityhub", "create-insight")]
public record AwsSecurityhubCreateInsightOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--filters")] string Filters,
[property: CliOption("--group-by-attribute")] string GroupByAttribute
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}