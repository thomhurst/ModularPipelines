using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("directconnect", "describe-router-configuration")]
public record AwsDirectconnectDescribeRouterConfigurationOptions(
[property: CliOption("--virtual-interface-id")] string VirtualInterfaceId
) : AwsOptions
{
    [CliOption("--router-type-identifier")]
    public string? RouterTypeIdentifier { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}