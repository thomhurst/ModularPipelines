using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloudcontrol", "get-resource-request-status")]
public record AwsCloudcontrolGetResourceRequestStatusOptions(
[property: CommandSwitch("--request-token")] string RequestToken
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}