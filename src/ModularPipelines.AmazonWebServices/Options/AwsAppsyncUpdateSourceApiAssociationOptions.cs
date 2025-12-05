using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("appsync", "update-source-api-association")]
public record AwsAppsyncUpdateSourceApiAssociationOptions(
[property: CliOption("--association-id")] string AssociationId,
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