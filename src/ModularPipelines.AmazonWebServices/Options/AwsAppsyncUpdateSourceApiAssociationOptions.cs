using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("appsync", "update-source-api-association")]
public record AwsAppsyncUpdateSourceApiAssociationOptions(
[property: CommandSwitch("--association-id")] string AssociationId,
[property: CommandSwitch("--merged-api-identifier")] string MergedApiIdentifier
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--source-api-association-config")]
    public string? SourceApiAssociationConfig { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}