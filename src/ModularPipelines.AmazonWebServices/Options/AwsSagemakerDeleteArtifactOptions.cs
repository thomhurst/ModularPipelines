using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker", "delete-artifact")]
public record AwsSagemakerDeleteArtifactOptions : AwsOptions
{
    [CommandSwitch("--artifact-arn")]
    public string? ArtifactArn { get; set; }

    [CommandSwitch("--source")]
    public string? Source { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}