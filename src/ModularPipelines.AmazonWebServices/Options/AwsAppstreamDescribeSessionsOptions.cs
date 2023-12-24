using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("appstream", "describe-sessions")]
public record AwsAppstreamDescribeSessionsOptions(
[property: CommandSwitch("--stack-name")] string StackName,
[property: CommandSwitch("--fleet-name")] string FleetName
) : AwsOptions
{
    [CommandSwitch("--user-id")]
    public string? UserId { get; set; }

    [CommandSwitch("--authentication-type")]
    public string? AuthenticationType { get; set; }

    [CommandSwitch("--instance-id")]
    public string? InstanceId { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}