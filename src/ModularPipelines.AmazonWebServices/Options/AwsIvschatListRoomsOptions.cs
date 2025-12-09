using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ivschat", "list-rooms")]
public record AwsIvschatListRoomsOptions : AwsOptions
{
    [CliOption("--logging-configuration-identifier")]
    public string? LoggingConfigurationIdentifier { get; set; }

    [CliOption("--max-results")]
    public int? MaxResults { get; set; }

    [CliOption("--message-review-handler-uri")]
    public string? MessageReviewHandlerUri { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}