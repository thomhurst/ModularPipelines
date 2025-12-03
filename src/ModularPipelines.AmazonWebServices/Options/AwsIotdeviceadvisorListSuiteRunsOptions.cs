using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iotdeviceadvisor", "list-suite-runs")]
public record AwsIotdeviceadvisorListSuiteRunsOptions : AwsOptions
{
    [CliOption("--suite-definition-id")]
    public string? SuiteDefinitionId { get; set; }

    [CliOption("--suite-definition-version")]
    public string? SuiteDefinitionVersion { get; set; }

    [CliOption("--max-results")]
    public int? MaxResults { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}