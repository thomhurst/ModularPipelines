using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iotdeviceadvisor", "list-suite-runs")]
public record AwsIotdeviceadvisorListSuiteRunsOptions : AwsOptions
{
    [CommandSwitch("--suite-definition-id")]
    public string? SuiteDefinitionId { get; set; }

    [CommandSwitch("--suite-definition-version")]
    public string? SuiteDefinitionVersion { get; set; }

    [CommandSwitch("--max-results")]
    public int? MaxResults { get; set; }

    [CommandSwitch("--next-token")]
    public string? NextToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}