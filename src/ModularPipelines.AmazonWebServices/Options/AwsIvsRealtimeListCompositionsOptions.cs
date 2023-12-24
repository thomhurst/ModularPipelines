using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ivs-realtime", "list-compositions")]
public record AwsIvsRealtimeListCompositionsOptions : AwsOptions
{
    [CommandSwitch("--filter-by-encoder-configuration-arn")]
    public string? FilterByEncoderConfigurationArn { get; set; }

    [CommandSwitch("--filter-by-stage-arn")]
    public string? FilterByStageArn { get; set; }

    [CommandSwitch("--max-results")]
    public int? MaxResults { get; set; }

    [CommandSwitch("--next-token")]
    public string? NextToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}