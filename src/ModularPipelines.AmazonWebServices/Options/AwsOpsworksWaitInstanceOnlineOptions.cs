using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("opsworks", "wait", "instance-online")]
public record AwsOpsworksWaitInstanceOnlineOptions : AwsOptions
{
    [CommandSwitch("--stack-id")]
    public string? StackId { get; set; }

    [CommandSwitch("--layer-id")]
    public string? LayerId { get; set; }

    [CommandSwitch("--instance-ids")]
    public string[]? InstanceIds { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}