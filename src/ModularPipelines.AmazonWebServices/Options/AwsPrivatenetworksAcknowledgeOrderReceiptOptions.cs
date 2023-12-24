using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("privatenetworks", "acknowledge-order-receipt")]
public record AwsPrivatenetworksAcknowledgeOrderReceiptOptions(
[property: CommandSwitch("--order-arn")] string OrderArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}