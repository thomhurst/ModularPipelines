using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lookoutequipment", "list-retraining-schedulers")]
public record AwsLookoutequipmentListRetrainingSchedulersOptions : AwsOptions
{
    [CliOption("--model-name-begins-with")]
    public string? ModelNameBeginsWith { get; set; }

    [CliOption("--status")]
    public string? Status { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--max-results")]
    public int? MaxResults { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}