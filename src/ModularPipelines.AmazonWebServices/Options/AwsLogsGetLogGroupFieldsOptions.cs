using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("logs", "get-log-group-fields")]
public record AwsLogsGetLogGroupFieldsOptions : AwsOptions
{
    [CommandSwitch("--log-group-name")]
    public string? LogGroupName { get; set; }

    [CommandSwitch("--time")]
    public long? Time { get; set; }

    [CommandSwitch("--log-group-identifier")]
    public string? LogGroupIdentifier { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}