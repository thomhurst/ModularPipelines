using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("logs", "create-log-group")]
public record AwsLogsCreateLogGroupOptions(
[property: CommandSwitch("--log-group-name")] string LogGroupName
) : AwsOptions
{
    [CommandSwitch("--kms-key-id")]
    public string? KmsKeyId { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--log-group-class")]
    public string? LogGroupClass { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}