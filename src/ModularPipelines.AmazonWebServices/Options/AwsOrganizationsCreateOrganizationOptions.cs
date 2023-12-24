using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("organizations", "create-organization")]
public record AwsOrganizationsCreateOrganizationOptions : AwsOptions
{
    [CommandSwitch("--feature-set")]
    public string? FeatureSet { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}