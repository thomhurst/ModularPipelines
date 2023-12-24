using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("securityhub", "start-configuration-policy-association")]
public record AwsSecurityhubStartConfigurationPolicyAssociationOptions(
[property: CommandSwitch("--configuration-policy-identifier")] string ConfigurationPolicyIdentifier,
[property: CommandSwitch("--target")] string Target
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}