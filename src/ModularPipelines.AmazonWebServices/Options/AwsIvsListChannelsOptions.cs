using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ivs", "list-channels")]
public record AwsIvsListChannelsOptions : AwsOptions
{
    [CommandSwitch("--filter-by-name")]
    public string? FilterByName { get; set; }

    [CommandSwitch("--filter-by-recording-configuration-arn")]
    public string? FilterByRecordingConfigurationArn { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}