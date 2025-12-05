using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sso-admin", "put-application-access-scope")]
public record AwsSsoAdminPutApplicationAccessScopeOptions(
[property: CliOption("--application-arn")] string ApplicationArn,
[property: CliOption("--scope")] string Scope
) : AwsOptions
{
    [CliOption("--authorized-targets")]
    public string[]? AuthorizedTargets { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}