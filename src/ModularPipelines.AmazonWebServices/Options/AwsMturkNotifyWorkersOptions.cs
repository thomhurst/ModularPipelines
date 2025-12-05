using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mturk", "notify-workers")]
public record AwsMturkNotifyWorkersOptions(
[property: CliOption("--subject")] string Subject,
[property: CliOption("--message-text")] string MessageText,
[property: CliOption("--worker-ids")] string[] WorkerIds
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}