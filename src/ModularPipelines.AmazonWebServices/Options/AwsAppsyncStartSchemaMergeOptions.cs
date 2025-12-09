using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("appsync", "start-schema-merge")]
public record AwsAppsyncStartSchemaMergeOptions(
[property: CliOption("--association-id")] string AssociationId,
[property: CliOption("--merged-api-identifier")] string MergedApiIdentifier
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}