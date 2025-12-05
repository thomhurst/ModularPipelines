using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sso-admin", "delete-application-grant")]
public record AwsSsoAdminDeleteApplicationGrantOptions(
[property: CliOption("--application-arn")] string ApplicationArn,
[property: CliOption("--grant-type")] string GrantType
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}