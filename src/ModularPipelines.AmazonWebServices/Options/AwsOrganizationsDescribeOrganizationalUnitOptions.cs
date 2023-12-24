using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("organizations", "describe-organizational-unit")]
public record AwsOrganizationsDescribeOrganizationalUnitOptions(
[property: CommandSwitch("--organizational-unit-id")] string OrganizationalUnitId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}