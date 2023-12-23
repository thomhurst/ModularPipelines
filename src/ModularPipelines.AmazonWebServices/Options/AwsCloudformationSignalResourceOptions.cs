using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloudformation", "signal-resource")]
public record AwsCloudformationSignalResourceOptions(
[property: CommandSwitch("--stack-name")] string StackName,
[property: CommandSwitch("--logical-resource-id")] string LogicalResourceId,
[property: CommandSwitch("--unique-id")] string UniqueId,
[property: CommandSwitch("--status")] string Status
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}