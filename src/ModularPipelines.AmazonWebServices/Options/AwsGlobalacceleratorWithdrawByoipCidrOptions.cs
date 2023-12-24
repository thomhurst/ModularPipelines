using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("globalaccelerator", "withdraw-byoip-cidr")]
public record AwsGlobalacceleratorWithdrawByoipCidrOptions(
[property: CommandSwitch("--cidr")] string Cidr
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}