using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("grafana", "list-permissions")]
public record AwsGrafanaListPermissionsOptions(
[property: CommandSwitch("--workspace-id")] string WorkspaceId
) : AwsOptions
{
    [CommandSwitch("--group-id")]
    public string? GroupId { get; set; }

    [CommandSwitch("--user-id")]
    public string? UserId { get; set; }

    [CommandSwitch("--user-type")]
    public string? UserType { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}