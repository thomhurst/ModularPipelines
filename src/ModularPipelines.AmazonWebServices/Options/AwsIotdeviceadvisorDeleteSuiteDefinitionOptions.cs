using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iotdeviceadvisor", "delete-suite-definition")]
public record AwsIotdeviceadvisorDeleteSuiteDefinitionOptions(
[property: CommandSwitch("--suite-definition-id")] string SuiteDefinitionId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}