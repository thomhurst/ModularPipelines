using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sso-admin", "delete-application-access-scope")]
public record AwsSsoAdminDeleteApplicationAccessScopeOptions(
[property: CliOption("--application-arn")] string ApplicationArn,
[property: CliOption("--scope")] string Scope
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}