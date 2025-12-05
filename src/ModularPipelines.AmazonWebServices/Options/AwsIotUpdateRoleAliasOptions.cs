using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "update-role-alias")]
public record AwsIotUpdateRoleAliasOptions(
[property: CliOption("--role-alias")] string RoleAlias
) : AwsOptions
{
    [CliOption("--role-arn")]
    public string? RoleArn { get; set; }

    [CliOption("--credential-duration-seconds")]
    public int? CredentialDurationSeconds { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}