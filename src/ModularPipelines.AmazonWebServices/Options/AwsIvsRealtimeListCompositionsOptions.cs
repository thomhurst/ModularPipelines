using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ivs-realtime", "list-compositions")]
public record AwsIvsRealtimeListCompositionsOptions : AwsOptions
{
    [CliOption("--filter-by-encoder-configuration-arn")]
    public string? FilterByEncoderConfigurationArn { get; set; }

    [CliOption("--filter-by-stage-arn")]
    public string? FilterByStageArn { get; set; }

    [CliOption("--max-results")]
    public int? MaxResults { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}