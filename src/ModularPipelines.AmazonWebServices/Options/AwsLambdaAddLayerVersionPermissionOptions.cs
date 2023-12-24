using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lambda", "add-layer-version-permission")]
public record AwsLambdaAddLayerVersionPermissionOptions(
[property: CommandSwitch("--layer-name")] string LayerName,
[property: CommandSwitch("--version-number")] long VersionNumber,
[property: CommandSwitch("--statement-id")] string StatementId,
[property: CommandSwitch("--action")] string Action,
[property: CommandSwitch("--principal")] string Principal
) : AwsOptions
{
    [CommandSwitch("--organization-id")]
    public string? OrganizationId { get; set; }

    [CommandSwitch("--revision-id")]
    public string? RevisionId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}