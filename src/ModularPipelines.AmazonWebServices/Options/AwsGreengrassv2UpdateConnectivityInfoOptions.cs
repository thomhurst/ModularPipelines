using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("greengrassv2", "update-connectivity-info")]
public record AwsGreengrassv2UpdateConnectivityInfoOptions(
[property: CliOption("--thing-name")] string ThingName,
[property: CliOption("--connectivity-info")] string[] ConnectivityInfo
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}