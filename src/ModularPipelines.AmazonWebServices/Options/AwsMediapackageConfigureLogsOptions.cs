using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mediapackage", "configure-logs")]
public record AwsMediapackageConfigureLogsOptions(
[property: CommandSwitch("--id")] string Id
) : AwsOptions
{
    [CommandSwitch("--egress-access-logs")]
    public string? EgressAccessLogs { get; set; }

    [CommandSwitch("--ingress-access-logs")]
    public string? IngressAccessLogs { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}