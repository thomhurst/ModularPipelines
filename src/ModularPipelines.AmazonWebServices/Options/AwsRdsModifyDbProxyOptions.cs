using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("rds", "modify-db-proxy")]
public record AwsRdsModifyDbProxyOptions(
[property: CliOption("--db-proxy-name")] string DbProxyName
) : AwsOptions
{
    [CliOption("--new-db-proxy-name")]
    public string? NewDbProxyName { get; set; }

    [CliOption("--auth")]
    public string[]? Auth { get; set; }

    [CliOption("--idle-client-timeout")]
    public int? IdleClientTimeout { get; set; }

    [CliOption("--role-arn")]
    public string? RoleArn { get; set; }

    [CliOption("--security-groups")]
    public string[]? SecurityGroups { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}