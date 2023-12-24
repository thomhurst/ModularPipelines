using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("logs", "put-retention-policy")]
public record AwsLogsPutRetentionPolicyOptions(
[property: CommandSwitch("--log-group-name")] string LogGroupName,
[property: CommandSwitch("--retention-in-days")] int RetentionInDays
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}