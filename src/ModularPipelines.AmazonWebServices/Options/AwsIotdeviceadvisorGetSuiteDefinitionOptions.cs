using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iotdeviceadvisor", "get-suite-definition")]
public record AwsIotdeviceadvisorGetSuiteDefinitionOptions(
[property: CliOption("--suite-definition-id")] string SuiteDefinitionId
) : AwsOptions
{
    [CliOption("--suite-definition-version")]
    public string? SuiteDefinitionVersion { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}