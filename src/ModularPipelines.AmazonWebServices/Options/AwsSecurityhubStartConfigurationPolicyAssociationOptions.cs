using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("securityhub", "start-configuration-policy-association")]
public record AwsSecurityhubStartConfigurationPolicyAssociationOptions(
[property: CliOption("--configuration-policy-identifier")] string ConfigurationPolicyIdentifier,
[property: CliOption("--target")] string Target
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}