using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("organizations", "enable-policy-type")]
public record AwsOrganizationsEnablePolicyTypeOptions(
[property: CommandSwitch("--root-id")] string RootId,
[property: CommandSwitch("--policy-type")] string PolicyType
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}