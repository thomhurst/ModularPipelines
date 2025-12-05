using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sesv2", "create-configuration-set")]
public record AwsSesv2CreateConfigurationSetOptions(
[property: CliOption("--configuration-set-name")] string ConfigurationSetName
) : AwsOptions
{
    [CliOption("--tracking-options")]
    public string? TrackingOptions { get; set; }

    [CliOption("--delivery-options")]
    public string? DeliveryOptions { get; set; }

    [CliOption("--reputation-options")]
    public string? ReputationOptions { get; set; }

    [CliOption("--sending-options")]
    public string? SendingOptions { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--suppression-options")]
    public string? SuppressionOptions { get; set; }

    [CliOption("--vdm-options")]
    public string? VdmOptions { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}