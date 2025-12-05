using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connectparticipant", "get-transcript")]
public record AwsConnectparticipantGetTranscriptOptions(
[property: CliOption("--connection-token")] string ConnectionToken
) : AwsOptions
{
    [CliOption("--contact-id")]
    public string? ContactId { get; set; }

    [CliOption("--max-results")]
    public int? MaxResults { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--scan-direction")]
    public string? ScanDirection { get; set; }

    [CliOption("--sort-order")]
    public string? SortOrder { get; set; }

    [CliOption("--start-position")]
    public string? StartPosition { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}