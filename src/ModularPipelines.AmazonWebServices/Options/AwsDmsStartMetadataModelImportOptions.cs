using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dms", "start-metadata-model-import")]
public record AwsDmsStartMetadataModelImportOptions(
[property: CliOption("--migration-project-identifier")] string MigrationProjectIdentifier,
[property: CliOption("--selection-rules")] string SelectionRules,
[property: CliOption("--origin")] string Origin
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}