using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lambda", "add-layer-version-permission")]
public record AwsLambdaAddLayerVersionPermissionOptions(
[property: CliOption("--layer-name")] string LayerName,
[property: CliOption("--version-number")] long VersionNumber,
[property: CliOption("--statement-id")] string StatementId,
[property: CliOption("--action")] string Action,
[property: CliOption("--principal")] string Principal
) : AwsOptions
{
    [CliOption("--organization-id")]
    public string? OrganizationId { get; set; }

    [CliOption("--revision-id")]
    public string? RevisionId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}