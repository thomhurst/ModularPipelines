using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ssm", "start-automation-execution")]
public record AwsSsmStartAutomationExecutionOptions(
[property: CommandSwitch("--document-name")] string DocumentName
) : AwsOptions
{
    [CommandSwitch("--document-version")]
    public string? DocumentVersion { get; set; }

    [CommandSwitch("--parameters")]
    public IEnumerable<KeyValue>? Parameters { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--mode")]
    public string? Mode { get; set; }

    [CommandSwitch("--target-parameter-name")]
    public string? TargetParameterName { get; set; }

    [CommandSwitch("--targets")]
    public string[]? Targets { get; set; }

    [CommandSwitch("--target-maps")]
    public string[]? TargetMaps { get; set; }

    [CommandSwitch("--max-concurrency")]
    public string? MaxConcurrency { get; set; }

    [CommandSwitch("--max-errors")]
    public string? MaxErrors { get; set; }

    [CommandSwitch("--target-locations")]
    public string[]? TargetLocations { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--alarm-configuration")]
    public string? AlarmConfiguration { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}