using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iotdeviceadvisor", "update-suite-definition")]
public record AwsIotdeviceadvisorUpdateSuiteDefinitionOptions(
[property: CliOption("--suite-definition-id")] string SuiteDefinitionId,
[property: CliOption("--suite-definition-configuration")] string SuiteDefinitionConfiguration
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}