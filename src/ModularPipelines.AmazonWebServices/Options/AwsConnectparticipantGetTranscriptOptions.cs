using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connectparticipant", "get-transcript")]
public record AwsConnectparticipantGetTranscriptOptions(
[property: CommandSwitch("--connection-token")] string ConnectionToken
) : AwsOptions
{
    [CommandSwitch("--contact-id")]
    public string? ContactId { get; set; }

    [CommandSwitch("--max-results")]
    public int? MaxResults { get; set; }

    [CommandSwitch("--next-token")]
    public string? NextToken { get; set; }

    [CommandSwitch("--scan-direction")]
    public string? ScanDirection { get; set; }

    [CommandSwitch("--sort-order")]
    public string? SortOrder { get; set; }

    [CommandSwitch("--start-position")]
    public string? StartPosition { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}