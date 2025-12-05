using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("securityhub", "update-configuration-policy")]
public record AwsSecurityhubUpdateConfigurationPolicyOptions(
[property: CliOption("--identifier")] string Identifier
) : AwsOptions
{
    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--updated-reason")]
    public string? UpdatedReason { get; set; }

    [CliOption("--configuration-policy")]
    public string? ConfigurationPolicy { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}