using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sesv2", "list-suppressed-destinations")]
public record AwsSesv2ListSuppressedDestinationsOptions : AwsOptions
{
    [CliOption("--reasons")]
    public string[]? Reasons { get; set; }

    [CliOption("--start-date")]
    public long? StartDate { get; set; }

    [CliOption("--end-date")]
    public long? EndDate { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}