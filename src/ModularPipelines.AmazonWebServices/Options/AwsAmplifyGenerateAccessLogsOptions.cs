using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("amplify", "generate-access-logs")]
public record AwsAmplifyGenerateAccessLogsOptions(
[property: CommandSwitch("--domain-name")] string DomainName,
[property: CommandSwitch("--app-id")] string AppId
) : AwsOptions
{
    [CommandSwitch("--start-time")]
    public long? StartTime { get; set; }

    [CommandSwitch("--end-time")]
    public long? EndTime { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}