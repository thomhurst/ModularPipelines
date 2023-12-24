using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("devicefarm", "renew-offering")]
public record AwsDevicefarmRenewOfferingOptions(
[property: CommandSwitch("--offering-id")] string OfferingId,
[property: CommandSwitch("--quantity")] int Quantity
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}