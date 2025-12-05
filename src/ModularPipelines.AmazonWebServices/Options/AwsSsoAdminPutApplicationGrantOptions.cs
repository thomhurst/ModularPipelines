using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sso-admin", "put-application-grant")]
public record AwsSsoAdminPutApplicationGrantOptions(
[property: CliOption("--application-arn")] string ApplicationArn,
[property: CliOption("--grant")] string Grant,
[property: CliOption("--grant-type")] string GrantType
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}