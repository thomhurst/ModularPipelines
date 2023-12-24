using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker", "create-artifact")]
public record AwsSagemakerCreateArtifactOptions(
[property: CommandSwitch("--source")] string Source,
[property: CommandSwitch("--artifact-type")] string ArtifactType
) : AwsOptions
{
    [CommandSwitch("--artifact-name")]
    public string? ArtifactName { get; set; }

    [CommandSwitch("--properties")]
    public IEnumerable<KeyValue>? Properties { get; set; }

    [CommandSwitch("--metadata-properties")]
    public string? MetadataProperties { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}