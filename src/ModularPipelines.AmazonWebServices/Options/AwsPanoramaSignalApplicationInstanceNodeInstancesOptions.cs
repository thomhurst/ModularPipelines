using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("panorama", "signal-application-instance-node-instances")]
public record AwsPanoramaSignalApplicationInstanceNodeInstancesOptions(
[property: CommandSwitch("--application-instance-id")] string ApplicationInstanceId,
[property: CommandSwitch("--node-signals")] string[] NodeSignals
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}