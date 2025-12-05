using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mediaconnect", "purchase-offering")]
public record AwsMediaconnectPurchaseOfferingOptions(
[property: CliOption("--offering-arn")] string OfferingArn,
[property: CliOption("--reservation-name")] string ReservationName,
[property: CliOption("--start")] string Start
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}