using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("panorama", "signal-application-instance-node-instances")]
public record AwsPanoramaSignalApplicationInstanceNodeInstancesOptions(
[property: CliOption("--application-instance-id")] string ApplicationInstanceId,
[property: CliOption("--node-signals")] string[] NodeSignals
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}