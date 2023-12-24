using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("proton", "list-deployments")]
public record AwsProtonListDeploymentsOptions : AwsOptions
{
    [CommandSwitch("--component-name")]
    public string? ComponentName { get; set; }

    [CommandSwitch("--environment-name")]
    public string? EnvironmentName { get; set; }

    [CommandSwitch("--service-instance-name")]
    public string? ServiceInstanceName { get; set; }

    [CommandSwitch("--service-name")]
    public string? ServiceName { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}