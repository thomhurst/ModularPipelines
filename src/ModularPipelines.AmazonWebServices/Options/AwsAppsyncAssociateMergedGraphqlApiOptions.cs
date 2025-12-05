using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("appsync", "associate-merged-graphql-api")]
public record AwsAppsyncAssociateMergedGraphqlApiOptions(
[property: CliOption("--source-api-identifier")] string SourceApiIdentifier,
[property: CliOption("--merged-api-identifier")] string MergedApiIdentifier
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--source-api-association-config")]
    public string? SourceApiAssociationConfig { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}