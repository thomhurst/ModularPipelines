using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "update-role-alias")]
public record AwsIotUpdateRoleAliasOptions(
[property: CommandSwitch("--role-alias")] string RoleAlias
) : AwsOptions
{
    [CommandSwitch("--role-arn")]
    public string? RoleArn { get; set; }

    [CommandSwitch("--credential-duration-seconds")]
    public int? CredentialDurationSeconds { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}