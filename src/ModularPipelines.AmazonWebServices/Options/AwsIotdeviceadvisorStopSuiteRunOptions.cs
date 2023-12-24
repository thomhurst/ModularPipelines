using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iotdeviceadvisor", "stop-suite-run")]
public record AwsIotdeviceadvisorStopSuiteRunOptions(
[property: CommandSwitch("--suite-definition-id")] string SuiteDefinitionId,
[property: CommandSwitch("--suite-run-id")] string SuiteRunId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}