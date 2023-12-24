using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("securityhub", "start-configuration-policy-disassociation")]
public record AwsSecurityhubStartConfigurationPolicyDisassociationOptions(
[property: CommandSwitch("--configuration-policy-identifier")] string ConfigurationPolicyIdentifier
) : AwsOptions
{
    [CommandSwitch("--target")]
    public string? Target { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}