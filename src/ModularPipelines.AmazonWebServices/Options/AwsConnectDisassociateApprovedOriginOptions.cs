using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connect", "disassociate-approved-origin")]
public record AwsConnectDisassociateApprovedOriginOptions(
[property: CommandSwitch("--instance-id")] string InstanceId,
[property: CommandSwitch("--origin")] string Origin
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}