using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workdocs", "remove-resource-permission")]
public record AwsWorkdocsRemoveResourcePermissionOptions(
[property: CommandSwitch("--resource-id")] string ResourceId,
[property: CommandSwitch("--principal-id")] string PrincipalId
) : AwsOptions
{
    [CommandSwitch("--authentication-token")]
    public string? AuthenticationToken { get; set; }

    [CommandSwitch("--principal-type")]
    public string? PrincipalType { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}