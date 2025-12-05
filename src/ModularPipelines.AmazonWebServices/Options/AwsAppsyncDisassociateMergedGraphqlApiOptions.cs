using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("appsync", "disassociate-merged-graphql-api")]
public record AwsAppsyncDisassociateMergedGraphqlApiOptions(
[property: CliOption("--source-api-identifier")] string SourceApiIdentifier,
[property: CliOption("--association-id")] string AssociationId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}