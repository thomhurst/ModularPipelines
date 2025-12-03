using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dms", "start-metadata-model-export-as-script")]
public record AwsDmsStartMetadataModelExportAsScriptOptions(
[property: CliOption("--migration-project-identifier")] string MigrationProjectIdentifier,
[property: CliOption("--selection-rules")] string SelectionRules,
[property: CliOption("--origin")] string Origin
) : AwsOptions
{
    [CliOption("--file-name")]
    public string? FileName { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}