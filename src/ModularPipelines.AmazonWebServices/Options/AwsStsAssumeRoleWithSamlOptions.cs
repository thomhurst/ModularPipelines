using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sts", "assume-role-with-saml")]
public record AwsStsAssumeRoleWithSamlOptions(
[property: CommandSwitch("--role-arn")] string RoleArn,
[property: CommandSwitch("--principal-arn")] string PrincipalArn,
[property: CommandSwitch("--saml-assertion")] string SamlAssertion
) : AwsOptions
{
    [CommandSwitch("--policy-arns")]
    public string[]? PolicyArns { get; set; }

    [CommandSwitch("--policy")]
    public string? Policy { get; set; }

    [CommandSwitch("--duration-seconds")]
    public int? DurationSeconds { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}