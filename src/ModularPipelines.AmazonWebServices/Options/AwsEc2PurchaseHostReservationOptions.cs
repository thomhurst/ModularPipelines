using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "purchase-host-reservation")]
public record AwsEc2PurchaseHostReservationOptions(
[property: CliOption("--host-id-set")] string[] HostIdSet,
[property: CliOption("--offering-id")] string OfferingId
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--currency-code")]
    public string? CurrencyCode { get; set; }

    [CliOption("--limit-price")]
    public string? LimitPrice { get; set; }

    [CliOption("--tag-specifications")]
    public string[]? TagSpecifications { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}