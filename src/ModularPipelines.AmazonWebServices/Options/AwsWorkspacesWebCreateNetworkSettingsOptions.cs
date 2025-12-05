using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workspaces-web", "create-network-settings")]
public record AwsWorkspacesWebCreateNetworkSettingsOptions(
[property: CliOption("--security-group-ids")] string[] SecurityGroupIds,
[property: CliOption("--subnet-ids")] string[] SubnetIds,
[property: CliOption("--vpc-id")] string VpcId
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}