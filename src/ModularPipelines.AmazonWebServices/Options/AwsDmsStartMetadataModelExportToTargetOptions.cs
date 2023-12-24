using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dms", "start-metadata-model-export-to-target")]
public record AwsDmsStartMetadataModelExportToTargetOptions(
[property: CommandSwitch("--migration-project-identifier")] string MigrationProjectIdentifier,
[property: CommandSwitch("--selection-rules")] string SelectionRules
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}