using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("appsync", "associate-source-graphql-api")]
public record AwsAppsyncAssociateSourceGraphqlApiOptions(
[property: CliOption("--merged-api-identifier")] string MergedApiIdentifier,
[property: CliOption("--source-api-identifier")] string SourceApiIdentifier
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--source-api-association-config")]
    public string? SourceApiAssociationConfig { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}