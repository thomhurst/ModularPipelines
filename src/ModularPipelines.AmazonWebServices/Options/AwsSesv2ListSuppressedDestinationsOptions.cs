using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sesv2", "list-suppressed-destinations")]
public record AwsSesv2ListSuppressedDestinationsOptions : AwsOptions
{
    [CommandSwitch("--reasons")]
    public string[]? Reasons { get; set; }

    [CommandSwitch("--start-date")]
    public long? StartDate { get; set; }

    [CommandSwitch("--end-date")]
    public long? EndDate { get; set; }

    [CommandSwitch("--next-token")]
    public string? NextToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}