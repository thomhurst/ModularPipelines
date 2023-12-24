using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ssm", "start-change-request-execution")]
public record AwsSsmStartChangeRequestExecutionOptions(
[property: CommandSwitch("--document-name")] string DocumentName,
[property: CommandSwitch("--runbooks")] string[] Runbooks
) : AwsOptions
{
    [CommandSwitch("--scheduled-time")]
    public long? ScheduledTime { get; set; }

    [CommandSwitch("--document-version")]
    public string? DocumentVersion { get; set; }

    [CommandSwitch("--parameters")]
    public IEnumerable<KeyValue>? Parameters { get; set; }

    [CommandSwitch("--change-request-name")]
    public string? ChangeRequestName { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--scheduled-end-time")]
    public long? ScheduledEndTime { get; set; }

    [CommandSwitch("--change-details")]
    public string? ChangeDetails { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}