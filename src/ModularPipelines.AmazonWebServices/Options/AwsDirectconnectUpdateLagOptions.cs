using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("directconnect", "update-lag")]
public record AwsDirectconnectUpdateLagOptions(
[property: CommandSwitch("--lag-id")] string LagId
) : AwsOptions
{
    [CommandSwitch("--lag-name")]
    public string? LagName { get; set; }

    [CommandSwitch("--minimum-links")]
    public int? MinimumLinks { get; set; }

    [CommandSwitch("--encryption-mode")]
    public string? EncryptionMode { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}