using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("configservice", "delete-organization-conformance-pack")]
public record AwsConfigserviceDeleteOrganizationConformancePackOptions(
[property: CommandSwitch("--organization-conformance-pack-name")] string OrganizationConformancePackName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}