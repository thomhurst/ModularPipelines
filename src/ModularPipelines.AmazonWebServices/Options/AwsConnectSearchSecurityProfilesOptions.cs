using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connect", "search-security-profiles")]
public record AwsConnectSearchSecurityProfilesOptions(
[property: CommandSwitch("--instance-id")] string InstanceId
) : AwsOptions
{
    [CommandSwitch("--search-criteria")]
    public string? SearchCriteria { get; set; }

    [CommandSwitch("--search-filter")]
    public string? SearchFilter { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}