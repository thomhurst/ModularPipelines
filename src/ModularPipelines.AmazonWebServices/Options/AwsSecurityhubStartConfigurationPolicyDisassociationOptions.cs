using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("securityhub", "start-configuration-policy-disassociation")]
public record AwsSecurityhubStartConfigurationPolicyDisassociationOptions(
[property: CliOption("--configuration-policy-identifier")] string ConfigurationPolicyIdentifier
) : AwsOptions
{
    [CliOption("--target")]
    public string? Target { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}