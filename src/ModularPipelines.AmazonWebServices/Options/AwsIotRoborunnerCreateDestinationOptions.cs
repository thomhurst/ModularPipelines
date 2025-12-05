using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot-roborunner", "create-destination")]
public record AwsIotRoborunnerCreateDestinationOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--site")] string Site
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--state")]
    public string? State { get; set; }

    [CliOption("--additional-fixed-properties")]
    public string? AdditionalFixedProperties { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}