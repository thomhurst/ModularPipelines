using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("devicefarm", "renew-offering")]
public record AwsDevicefarmRenewOfferingOptions(
[property: CliOption("--offering-id")] string OfferingId,
[property: CliOption("--quantity")] int Quantity
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}