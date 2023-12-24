using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloudcontrol", "wait", "resource-request-success")]
public record AwsCloudcontrolWaitResourceRequestSuccessOptions(
[property: CommandSwitch("--request-token")] string RequestToken
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}