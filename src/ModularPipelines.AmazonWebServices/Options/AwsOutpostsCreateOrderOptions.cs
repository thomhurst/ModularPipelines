using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("outposts", "create-order")]
public record AwsOutpostsCreateOrderOptions(
[property: CliOption("--outpost-identifier")] string OutpostIdentifier,
[property: CliOption("--line-items")] string[] LineItems,
[property: CliOption("--payment-option")] string PaymentOption
) : AwsOptions
{
    [CliOption("--payment-term")]
    public string? PaymentTerm { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}