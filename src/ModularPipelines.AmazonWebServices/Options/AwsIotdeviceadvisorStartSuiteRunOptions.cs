using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iotdeviceadvisor", "start-suite-run")]
public record AwsIotdeviceadvisorStartSuiteRunOptions(
[property: CliOption("--suite-definition-id")] string SuiteDefinitionId,
[property: CliOption("--suite-run-configuration")] string SuiteRunConfiguration
) : AwsOptions
{
    [CliOption("--suite-definition-version")]
    public string? SuiteDefinitionVersion { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}