using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("appsync", "list-types-by-association")]
public record AwsAppsyncListTypesByAssociationOptions(
[property: CliOption("--merged-api-identifier")] string MergedApiIdentifier,
[property: CliOption("--association-id")] string AssociationId,
[property: CliOption("--format")] string Format
) : AwsOptions
{
    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--max-results")]
    public int? MaxResults { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}