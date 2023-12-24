using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("organizations", "delete-organizational-unit")]
public record AwsOrganizationsDeleteOrganizationalUnitOptions(
[property: CommandSwitch("--organizational-unit-id")] string OrganizationalUnitId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}