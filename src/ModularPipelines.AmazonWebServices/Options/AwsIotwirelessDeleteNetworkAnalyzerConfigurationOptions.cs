using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iotwireless", "delete-network-analyzer-configuration")]
public record AwsIotwirelessDeleteNetworkAnalyzerConfigurationOptions(
[property: CliOption("--configuration-name")] string ConfigurationName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}