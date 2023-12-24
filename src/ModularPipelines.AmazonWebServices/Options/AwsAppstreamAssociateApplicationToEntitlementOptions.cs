using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("appstream", "associate-application-to-entitlement")]
public record AwsAppstreamAssociateApplicationToEntitlementOptions(
[property: CommandSwitch("--stack-name")] string StackName,
[property: CommandSwitch("--entitlement-name")] string EntitlementName,
[property: CommandSwitch("--application-identifier")] string ApplicationIdentifier
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}