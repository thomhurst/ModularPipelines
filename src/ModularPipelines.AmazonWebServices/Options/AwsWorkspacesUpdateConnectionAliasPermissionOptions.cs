using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workspaces", "update-connection-alias-permission")]
public record AwsWorkspacesUpdateConnectionAliasPermissionOptions(
[property: CommandSwitch("--alias-id")] string AliasId,
[property: CommandSwitch("--connection-alias-permission")] string ConnectionAliasPermission
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}