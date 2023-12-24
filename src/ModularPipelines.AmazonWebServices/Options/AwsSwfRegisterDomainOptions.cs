using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("swf", "register-domain")]
public record AwsSwfRegisterDomainOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--workflow-execution-retention-period-in-days")] string WorkflowExecutionRetentionPeriodInDays
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}