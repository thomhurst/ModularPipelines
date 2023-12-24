using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("rolesanywhere", "disable-trust-anchor")]
public record AwsRolesanywhereDisableTrustAnchorOptions(
[property: CommandSwitch("--trust-anchor-id")] string TrustAnchorId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}