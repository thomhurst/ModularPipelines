using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iotdeviceadvisor", "get-suite-definition")]
public record AwsIotdeviceadvisorGetSuiteDefinitionOptions(
[property: CommandSwitch("--suite-definition-id")] string SuiteDefinitionId
) : AwsOptions
{
    [CommandSwitch("--suite-definition-version")]
    public string? SuiteDefinitionVersion { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}