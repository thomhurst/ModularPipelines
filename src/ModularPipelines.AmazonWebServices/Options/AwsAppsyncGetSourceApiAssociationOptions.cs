using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("appsync", "get-source-api-association")]
public record AwsAppsyncGetSourceApiAssociationOptions(
[property: CliOption("--merged-api-identifier")] string MergedApiIdentifier,
[property: CliOption("--association-id")] string AssociationId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}