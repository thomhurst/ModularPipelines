using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sso-admin", "get-application-grant")]
public record AwsSsoAdminGetApplicationGrantOptions(
[property: CliOption("--application-arn")] string ApplicationArn,
[property: CliOption("--grant-type")] string GrantType
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}