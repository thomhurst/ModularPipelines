using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("greengrassv2", "get-component-version-artifact")]
public record AwsGreengrassv2GetComponentVersionArtifactOptions(
[property: CommandSwitch("--arn")] string Arn,
[property: CommandSwitch("--artifact-name")] string ArtifactName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}