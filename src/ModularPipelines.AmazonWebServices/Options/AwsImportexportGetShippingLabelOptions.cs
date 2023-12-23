using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("importexport", "get-shipping-label")]
public record AwsImportexportGetShippingLabelOptions(
[property: CommandSwitch("--job-ids")] string[] JobIds
) : AwsOptions
{
    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--company")]
    public string? Company { get; set; }

    [CommandSwitch("--phone-number")]
    public string? PhoneNumber { get; set; }

    [CommandSwitch("--country")]
    public string? Country { get; set; }

    [CommandSwitch("--state-or-province")]
    public string? StateOrProvince { get; set; }

    [CommandSwitch("--city")]
    public string? City { get; set; }

    [CommandSwitch("--postal-code")]
    public string? PostalCode { get; set; }

    [CommandSwitch("--street1")]
    public string? Street1 { get; set; }

    [CommandSwitch("--street2")]
    public string? Street2 { get; set; }

    [CommandSwitch("--street3")]
    public string? Street3 { get; set; }

    [CommandSwitch("--api-version")]
    public string? ApiVersion { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}