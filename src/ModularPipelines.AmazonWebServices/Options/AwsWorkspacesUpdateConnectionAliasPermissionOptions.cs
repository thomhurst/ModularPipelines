using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workspaces", "update-connection-alias-permission")]
public record AwsWorkspacesUpdateConnectionAliasPermissionOptions(
[property: CliOption("--alias-id")] string AliasId,
[property: CliOption("--connection-alias-permission")] string ConnectionAliasPermission
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}