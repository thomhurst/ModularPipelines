using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sesv2", "create-configuration-set")]
public record AwsSesv2CreateConfigurationSetOptions(
[property: CommandSwitch("--configuration-set-name")] string ConfigurationSetName
) : AwsOptions
{
    [CommandSwitch("--tracking-options")]
    public string? TrackingOptions { get; set; }

    [CommandSwitch("--delivery-options")]
    public string? DeliveryOptions { get; set; }

    [CommandSwitch("--reputation-options")]
    public string? ReputationOptions { get; set; }

    [CommandSwitch("--sending-options")]
    public string? SendingOptions { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--suppression-options")]
    public string? SuppressionOptions { get; set; }

    [CommandSwitch("--vdm-options")]
    public string? VdmOptions { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}