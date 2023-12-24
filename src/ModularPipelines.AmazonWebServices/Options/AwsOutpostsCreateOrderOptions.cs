using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("outposts", "create-order")]
public record AwsOutpostsCreateOrderOptions(
[property: CommandSwitch("--outpost-identifier")] string OutpostIdentifier,
[property: CommandSwitch("--line-items")] string[] LineItems,
[property: CommandSwitch("--payment-option")] string PaymentOption
) : AwsOptions
{
    [CommandSwitch("--payment-term")]
    public string? PaymentTerm { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}