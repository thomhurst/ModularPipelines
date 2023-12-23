using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lakeformation", "assume-decorated-role-with-saml")]
public record AwsLakeformationAssumeDecoratedRoleWithSamlOptions(
[property: CommandSwitch("--saml-assertion")] string SamlAssertion,
[property: CommandSwitch("--role-arn")] string RoleArn,
[property: CommandSwitch("--principal-arn")] string PrincipalArn
) : AwsOptions
{
    [CommandSwitch("--duration-seconds")]
    public int? DurationSeconds { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}