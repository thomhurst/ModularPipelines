using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connect", "list-rules")]
public record AwsConnectListRulesOptions(
[property: CommandSwitch("--instance-id")] string InstanceId
) : AwsOptions
{
    [CommandSwitch("--publish-status")]
    public string? PublishStatus { get; set; }

    [CommandSwitch("--event-source-name")]
    public string? EventSourceName { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}