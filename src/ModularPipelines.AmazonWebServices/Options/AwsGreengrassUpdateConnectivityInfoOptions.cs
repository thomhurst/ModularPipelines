using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("greengrass", "update-connectivity-info")]
public record AwsGreengrassUpdateConnectivityInfoOptions(
[property: CliOption("--thing-name")] string ThingName
) : AwsOptions
{
    [CliOption("--connectivity-info")]
    public string[]? ConnectivityInfo { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}