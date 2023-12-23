using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "start-on-demand-audit-task")]
public record AwsIotStartOnDemandAuditTaskOptions(
[property: CommandSwitch("--target-check-names")] string[] TargetCheckNames
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}