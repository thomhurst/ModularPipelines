using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("proton", "update-environment-account-connection")]
public record AwsProtonUpdateEnvironmentAccountConnectionOptions(
[property: CliOption("--id")] string Id
) : AwsOptions
{
    [CliOption("--codebuild-role-arn")]
    public string? CodebuildRoleArn { get; set; }

    [CliOption("--component-role-arn")]
    public string? ComponentRoleArn { get; set; }

    [CliOption("--role-arn")]
    public string? RoleArn { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}