using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("proton", "create-environment-account-connection")]
public record AwsProtonCreateEnvironmentAccountConnectionOptions(
[property: CliOption("--environment-name")] string EnvironmentName,
[property: CliOption("--management-account-id")] string ManagementAccountId
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--codebuild-role-arn")]
    public string? CodebuildRoleArn { get; set; }

    [CliOption("--component-role-arn")]
    public string? ComponentRoleArn { get; set; }

    [CliOption("--role-arn")]
    public string? RoleArn { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}