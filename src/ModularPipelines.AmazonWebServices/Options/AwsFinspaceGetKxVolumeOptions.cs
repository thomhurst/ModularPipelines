using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("finspace", "get-kx-volume")]
public record AwsFinspaceGetKxVolumeOptions(
[property: CommandSwitch("--environment-id")] string EnvironmentId,
[property: CommandSwitch("--volume-name")] string VolumeName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}