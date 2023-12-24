using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mediaconnect", "purchase-offering")]
public record AwsMediaconnectPurchaseOfferingOptions(
[property: CommandSwitch("--offering-arn")] string OfferingArn,
[property: CommandSwitch("--reservation-name")] string ReservationName,
[property: CommandSwitch("--start")] string Start
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}