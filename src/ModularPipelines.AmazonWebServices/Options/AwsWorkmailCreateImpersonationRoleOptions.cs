using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workmail", "create-impersonation-role")]
public record AwsWorkmailCreateImpersonationRoleOptions(
[property: CommandSwitch("--organization-id")] string OrganizationId,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--type")] string Type,
[property: CommandSwitch("--rules")] string[] Rules
) : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}