using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lakeformation", "assume-decorated-role-with-saml")]
public record AwsLakeformationAssumeDecoratedRoleWithSamlOptions(
[property: CliOption("--saml-assertion")] string SamlAssertion,
[property: CliOption("--role-arn")] string RoleArn,
[property: CliOption("--principal-arn")] string PrincipalArn
) : AwsOptions
{
    [CliOption("--duration-seconds")]
    public int? DurationSeconds { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}