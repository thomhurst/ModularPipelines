using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("datazone", "list-notifications")]
public record AwsDatazoneListNotificationsOptions(
[property: CliOption("--domain-identifier")] string DomainIdentifier,
[property: CliOption("--type")] string Type
) : AwsOptions
{
    [CliOption("--after-timestamp")]
    public long? AfterTimestamp { get; set; }

    [CliOption("--before-timestamp")]
    public long? BeforeTimestamp { get; set; }

    [CliOption("--subjects")]
    public string[]? Subjects { get; set; }

    [CliOption("--task-status")]
    public string? TaskStatus { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}