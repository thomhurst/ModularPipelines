using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mgn", "update-wave")]
public record AwsMgnUpdateWaveOptions(
[property: CommandSwitch("--wave-id")] string WaveId
) : AwsOptions
{
    [CommandSwitch("--account-id")]
    public string? AccountId { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}