using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iotwireless", "get-service-endpoint")]
public record AwsIotwirelessGetServiceEndpointOptions : AwsOptions
{
    [CliOption("--service-type")]
    public string? ServiceType { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}