using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "create-role-alias")]
public record AwsIotCreateRoleAliasOptions(
[property: CliOption("--role-alias")] string RoleAlias,
[property: CliOption("--role-arn")] string RoleArn
) : AwsOptions
{
    [CliOption("--credential-duration-seconds")]
    public int? CredentialDurationSeconds { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}