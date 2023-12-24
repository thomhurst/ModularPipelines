using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("servicediscovery", "get-instances-health-status")]
public record AwsServicediscoveryGetInstancesHealthStatusOptions(
[property: CommandSwitch("--service-id")] string ServiceId
) : AwsOptions
{
    [CommandSwitch("--instances")]
    public string[]? Instances { get; set; }

    [CommandSwitch("--max-results")]
    public int? MaxResults { get; set; }

    [CommandSwitch("--next-token")]
    public string? NextToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}