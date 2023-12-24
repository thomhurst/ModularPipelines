using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("appflow", "describe-connector-profiles")]
public record AwsAppflowDescribeConnectorProfilesOptions : AwsOptions
{
    [CommandSwitch("--connector-profile-names")]
    public string[]? ConnectorProfileNames { get; set; }

    [CommandSwitch("--connector-type")]
    public string? ConnectorType { get; set; }

    [CommandSwitch("--connector-label")]
    public string? ConnectorLabel { get; set; }

    [CommandSwitch("--max-results")]
    public int? MaxResults { get; set; }

    [CommandSwitch("--next-token")]
    public string? NextToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}