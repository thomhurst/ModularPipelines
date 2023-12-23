using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iotdeviceadvisor", "update-suite-definition")]
public record AwsIotdeviceadvisorUpdateSuiteDefinitionOptions(
[property: CommandSwitch("--suite-definition-id")] string SuiteDefinitionId,
[property: CommandSwitch("--suite-definition-configuration")] string SuiteDefinitionConfiguration
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}