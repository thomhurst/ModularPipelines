using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pricing", "list-price-lists")]
public record AwsPricingListPriceListsOptions(
[property: CommandSwitch("--service-code")] string ServiceCode,
[property: CommandSwitch("--effective-date")] long EffectiveDate,
[property: CommandSwitch("--currency-code")] string CurrencyCode
) : AwsOptions
{
    [CommandSwitch("--region-code")]
    public string? RegionCode { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}