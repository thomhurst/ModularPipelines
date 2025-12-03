using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("macie2", "create-classification-job")]
public record AwsMacie2CreateClassificationJobOptions(
[property: CliOption("--job-type")] string JobType,
[property: CliOption("--name")] string Name,
[property: CliOption("--s3-job-definition")] string S3JobDefinition
) : AwsOptions
{
    [CliOption("--allow-list-ids")]
    public string[]? AllowListIds { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--custom-data-identifier-ids")]
    public string[]? CustomDataIdentifierIds { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--managed-data-identifier-ids")]
    public string[]? ManagedDataIdentifierIds { get; set; }

    [CliOption("--managed-data-identifier-selector")]
    public string? ManagedDataIdentifierSelector { get; set; }

    [CliOption("--sampling-percentage")]
    public int? SamplingPercentage { get; set; }

    [CliOption("--schedule-frequency")]
    public string? ScheduleFrequency { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}