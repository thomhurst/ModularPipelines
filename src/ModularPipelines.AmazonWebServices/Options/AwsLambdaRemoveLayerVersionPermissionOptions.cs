using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lambda", "remove-layer-version-permission")]
public record AwsLambdaRemoveLayerVersionPermissionOptions(
[property: CommandSwitch("--layer-name")] string LayerName,
[property: CommandSwitch("--version-number")] long VersionNumber,
[property: CommandSwitch("--statement-id")] string StatementId
) : AwsOptions
{
    [CommandSwitch("--revision-id")]
    public string? RevisionId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}