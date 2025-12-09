using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("importexport", "get-shipping-label")]
public record AwsImportexportGetShippingLabelOptions(
[property: CliOption("--job-ids")] string[] JobIds
) : AwsOptions
{
    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--company")]
    public string? Company { get; set; }

    [CliOption("--phone-number")]
    public string? PhoneNumber { get; set; }

    [CliOption("--country")]
    public string? Country { get; set; }

    [CliOption("--state-or-province")]
    public string? StateOrProvince { get; set; }

    [CliOption("--city")]
    public string? City { get; set; }

    [CliOption("--postal-code")]
    public string? PostalCode { get; set; }

    [CliOption("--street1")]
    public string? Street1 { get; set; }

    [CliOption("--street2")]
    public string? Street2 { get; set; }

    [CliOption("--street3")]
    public string? Street3 { get; set; }

    [CliOption("--api-version")]
    public string? ApiVersion { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}