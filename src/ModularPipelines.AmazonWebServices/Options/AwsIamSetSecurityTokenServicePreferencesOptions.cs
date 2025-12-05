using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iam", "set-security-token-service-preferences")]
public record AwsIamSetSecurityTokenServicePreferencesOptions(
[property: CliOption("--global-endpoint-token-version")] string GlobalEndpointTokenVersion
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}