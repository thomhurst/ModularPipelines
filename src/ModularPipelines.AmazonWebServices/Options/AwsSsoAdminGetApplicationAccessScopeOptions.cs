using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sso-admin", "get-application-access-scope")]
public record AwsSsoAdminGetApplicationAccessScopeOptions(
[property: CliOption("--application-arn")] string ApplicationArn,
[property: CliOption("--scope")] string Scope
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}