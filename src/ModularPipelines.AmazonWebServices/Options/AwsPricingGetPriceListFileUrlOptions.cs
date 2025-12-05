using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pricing", "get-price-list-file-url")]
public record AwsPricingGetPriceListFileUrlOptions(
[property: CliOption("--price-list-arn")] string PriceListArn,
[property: CliOption("--file-format")] string FileFormat
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}