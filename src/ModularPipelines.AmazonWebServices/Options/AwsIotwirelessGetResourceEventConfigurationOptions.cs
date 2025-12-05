using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iotwireless", "get-resource-event-configuration")]
public record AwsIotwirelessGetResourceEventConfigurationOptions(
[property: CliOption("--identifier")] string Identifier,
[property: CliOption("--identifier-type")] string IdentifierType
) : AwsOptions
{
    [CliOption("--partner-type")]
    public string? PartnerType { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}