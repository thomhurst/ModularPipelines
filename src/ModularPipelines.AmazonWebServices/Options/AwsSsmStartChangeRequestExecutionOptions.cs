using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ssm", "start-change-request-execution")]
public record AwsSsmStartChangeRequestExecutionOptions(
[property: CliOption("--document-name")] string DocumentName,
[property: CliOption("--runbooks")] string[] Runbooks
) : AwsOptions
{
    [CliOption("--scheduled-time")]
    public long? ScheduledTime { get; set; }

    [CliOption("--document-version")]
    public string? DocumentVersion { get; set; }

    [CliOption("--parameters")]
    public IEnumerable<KeyValue>? Parameters { get; set; }

    [CliOption("--change-request-name")]
    public string? ChangeRequestName { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--scheduled-end-time")]
    public long? ScheduledEndTime { get; set; }

    [CliOption("--change-details")]
    public string? ChangeDetails { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}