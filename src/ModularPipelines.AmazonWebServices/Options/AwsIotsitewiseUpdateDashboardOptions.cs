using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iotsitewise", "update-dashboard")]
public record AwsIotsitewiseUpdateDashboardOptions(
[property: CommandSwitch("--dashboard-id")] string DashboardId,
[property: CommandSwitch("--dashboard-name")] string DashboardName,
[property: CommandSwitch("--dashboard-definition")] string DashboardDefinition
) : AwsOptions
{
    [CommandSwitch("--dashboard-description")]
    public string? DashboardDescription { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}