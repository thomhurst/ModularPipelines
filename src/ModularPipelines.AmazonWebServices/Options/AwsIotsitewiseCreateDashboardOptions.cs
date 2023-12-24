using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iotsitewise", "create-dashboard")]
public record AwsIotsitewiseCreateDashboardOptions(
[property: CommandSwitch("--project-id")] string ProjectId,
[property: CommandSwitch("--dashboard-name")] string DashboardName,
[property: CommandSwitch("--dashboard-definition")] string DashboardDefinition
) : AwsOptions
{
    [CommandSwitch("--dashboard-description")]
    public string? DashboardDescription { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}