using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sts", "assume-role-with-saml")]
public record AwsStsAssumeRoleWithSamlOptions(
[property: CliOption("--role-arn")] string RoleArn,
[property: CliOption("--principal-arn")] string PrincipalArn,
[property: CliOption("--saml-assertion")] string SamlAssertion
) : AwsOptions
{
    [CliOption("--policy-arns")]
    public string[]? PolicyArns { get; set; }

    [CliOption("--policy")]
    public string? Policy { get; set; }

    [CliOption("--duration-seconds")]
    public int? DurationSeconds { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}