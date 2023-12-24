using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("macie2", "create-classification-job")]
public record AwsMacie2CreateClassificationJobOptions(
[property: CommandSwitch("--job-type")] string JobType,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--s3-job-definition")] string S3JobDefinition
) : AwsOptions
{
    [CommandSwitch("--allow-list-ids")]
    public string[]? AllowListIds { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--custom-data-identifier-ids")]
    public string[]? CustomDataIdentifierIds { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--managed-data-identifier-ids")]
    public string[]? ManagedDataIdentifierIds { get; set; }

    [CommandSwitch("--managed-data-identifier-selector")]
    public string? ManagedDataIdentifierSelector { get; set; }

    [CommandSwitch("--sampling-percentage")]
    public int? SamplingPercentage { get; set; }

    [CommandSwitch("--schedule-frequency")]
    public string? ScheduleFrequency { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}