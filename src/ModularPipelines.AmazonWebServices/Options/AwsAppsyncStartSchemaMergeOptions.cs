using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("appsync", "start-schema-merge")]
public record AwsAppsyncStartSchemaMergeOptions(
[property: CommandSwitch("--association-id")] string AssociationId,
[property: CommandSwitch("--merged-api-identifier")] string MergedApiIdentifier
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}