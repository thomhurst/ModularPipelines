using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "purchase-host-reservation")]
public record AwsEc2PurchaseHostReservationOptions(
[property: CommandSwitch("--host-id-set")] string[] HostIdSet,
[property: CommandSwitch("--offering-id")] string OfferingId
) : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--currency-code")]
    public string? CurrencyCode { get; set; }

    [CommandSwitch("--limit-price")]
    public string? LimitPrice { get; set; }

    [CommandSwitch("--tag-specifications")]
    public string[]? TagSpecifications { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}