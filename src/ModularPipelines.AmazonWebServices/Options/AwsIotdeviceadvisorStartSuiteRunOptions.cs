using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iotdeviceadvisor", "start-suite-run")]
public record AwsIotdeviceadvisorStartSuiteRunOptions(
[property: CommandSwitch("--suite-definition-id")] string SuiteDefinitionId,
[property: CommandSwitch("--suite-run-configuration")] string SuiteRunConfiguration
) : AwsOptions
{
    [CommandSwitch("--suite-definition-version")]
    public string? SuiteDefinitionVersion { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}