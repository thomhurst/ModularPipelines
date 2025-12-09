using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("medialive", "batch-update-schedule")]
public record AwsMedialiveBatchUpdateScheduleOptions(
[property: CliOption("--channel-id")] string ChannelId
) : AwsOptions
{
    [CliOption("--creates")]
    public string? Creates { get; set; }

    [CliOption("--deletes")]
    public string? Deletes { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}