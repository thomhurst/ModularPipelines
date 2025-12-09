using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pinpoint-email", "create-configuration-set")]
public record AwsPinpointEmailCreateConfigurationSetOptions(
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

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}