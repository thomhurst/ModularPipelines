using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("appstream", "disassociate-application-from-entitlement")]
public record AwsAppstreamDisassociateApplicationFromEntitlementOptions(
[property: CommandSwitch("--stack-name")] string StackName,
[property: CommandSwitch("--entitlement-name")] string EntitlementName,
[property: CommandSwitch("--application-identifier")] string ApplicationIdentifier
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}