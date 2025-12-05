using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lambda", "remove-layer-version-permission")]
public record AwsLambdaRemoveLayerVersionPermissionOptions(
[property: CliOption("--layer-name")] string LayerName,
[property: CliOption("--version-number")] long VersionNumber,
[property: CliOption("--statement-id")] string StatementId
) : AwsOptions
{
    [CliOption("--revision-id")]
    public string? RevisionId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}