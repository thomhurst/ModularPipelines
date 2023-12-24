using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("securityhub", "batch-get-configuration-policy-associations")]
public record AwsSecurityhubBatchGetConfigurationPolicyAssociationsOptions(
[property: CommandSwitch("--configuration-policy-association-identifiers")] string[] ConfigurationPolicyAssociationIdentifiers
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}