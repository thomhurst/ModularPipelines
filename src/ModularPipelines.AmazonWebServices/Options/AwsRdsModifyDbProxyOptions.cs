using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("rds", "modify-db-proxy")]
public record AwsRdsModifyDbProxyOptions(
[property: CommandSwitch("--db-proxy-name")] string DbProxyName
) : AwsOptions
{
    [CommandSwitch("--new-db-proxy-name")]
    public string? NewDbProxyName { get; set; }

    [CommandSwitch("--auth")]
    public string[]? Auth { get; set; }

    [CommandSwitch("--idle-client-timeout")]
    public int? IdleClientTimeout { get; set; }

    [CommandSwitch("--role-arn")]
    public string? RoleArn { get; set; }

    [CommandSwitch("--security-groups")]
    public string[]? SecurityGroups { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}