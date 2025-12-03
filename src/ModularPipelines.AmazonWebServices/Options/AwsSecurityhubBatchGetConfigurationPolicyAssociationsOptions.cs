using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("securityhub", "batch-get-configuration-policy-associations")]
public record AwsSecurityhubBatchGetConfigurationPolicyAssociationsOptions(
[property: CliOption("--configuration-policy-association-identifiers")] string[] ConfigurationPolicyAssociationIdentifiers
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}