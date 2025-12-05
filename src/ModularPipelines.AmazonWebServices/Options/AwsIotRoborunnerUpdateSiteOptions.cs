using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot-roborunner", "update-site")]
public record AwsIotRoborunnerUpdateSiteOptions(
[property: CliOption("--id")] string Id
) : AwsOptions
{
    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--country-code")]
    public string? CountryCode { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}