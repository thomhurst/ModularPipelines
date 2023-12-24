using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iotdeviceadvisor", "get-suite-run-report")]
public record AwsIotdeviceadvisorGetSuiteRunReportOptions(
[property: CommandSwitch("--suite-definition-id")] string SuiteDefinitionId,
[property: CommandSwitch("--suite-run-id")] string SuiteRunId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}