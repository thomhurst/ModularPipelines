using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ssm", "start-automation-execution")]
public record AwsSsmStartAutomationExecutionOptions(
[property: CliOption("--document-name")] string DocumentName
) : AwsOptions
{
    [CliOption("--document-version")]
    public string? DocumentVersion { get; set; }

    [CliOption("--parameters")]
    public IEnumerable<KeyValue>? Parameters { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--mode")]
    public string? Mode { get; set; }

    [CliOption("--target-parameter-name")]
    public string? TargetParameterName { get; set; }

    [CliOption("--targets")]
    public string[]? Targets { get; set; }

    [CliOption("--target-maps")]
    public string[]? TargetMaps { get; set; }

    [CliOption("--max-concurrency")]
    public string? MaxConcurrency { get; set; }

    [CliOption("--max-errors")]
    public string? MaxErrors { get; set; }

    [CliOption("--target-locations")]
    public string[]? TargetLocations { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--alarm-configuration")]
    public string? AlarmConfiguration { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}