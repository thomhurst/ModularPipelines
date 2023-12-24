using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker", "update-artifact")]
public record AwsSagemakerUpdateArtifactOptions(
[property: CommandSwitch("--artifact-arn")] string ArtifactArn
) : AwsOptions
{
    [CommandSwitch("--artifact-name")]
    public string? ArtifactName { get; set; }

    [CommandSwitch("--properties")]
    public IEnumerable<KeyValue>? Properties { get; set; }

    [CommandSwitch("--properties-to-remove")]
    public string[]? PropertiesToRemove { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}