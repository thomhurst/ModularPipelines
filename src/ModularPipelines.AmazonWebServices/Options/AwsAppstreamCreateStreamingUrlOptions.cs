using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("appstream", "create-streaming-url")]
public record AwsAppstreamCreateStreamingUrlOptions(
[property: CommandSwitch("--stack-name")] string StackName,
[property: CommandSwitch("--fleet-name")] string FleetName,
[property: CommandSwitch("--user-id")] string UserId
) : AwsOptions
{
    [CommandSwitch("--application-id")]
    public string? ApplicationId { get; set; }

    [CommandSwitch("--validity")]
    public long? Validity { get; set; }

    [CommandSwitch("--session-context")]
    public string? SessionContext { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}