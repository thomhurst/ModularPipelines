using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pricing", "get-price-list-file-url")]
public record AwsPricingGetPriceListFileUrlOptions(
[property: CommandSwitch("--price-list-arn")] string PriceListArn,
[property: CommandSwitch("--file-format")] string FileFormat
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}