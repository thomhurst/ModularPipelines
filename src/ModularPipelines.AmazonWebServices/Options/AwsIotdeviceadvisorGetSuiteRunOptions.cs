using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iotdeviceadvisor", "get-suite-run")]
public record AwsIotdeviceadvisorGetSuiteRunOptions(
[property: CliOption("--suite-definition-id")] string SuiteDefinitionId,
[property: CliOption("--suite-run-id")] string SuiteRunId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}