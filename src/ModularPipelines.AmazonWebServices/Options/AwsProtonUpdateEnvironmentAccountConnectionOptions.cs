using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("proton", "update-environment-account-connection")]
public record AwsProtonUpdateEnvironmentAccountConnectionOptions(
[property: CommandSwitch("--id")] string Id
) : AwsOptions
{
    [CommandSwitch("--codebuild-role-arn")]
    public string? CodebuildRoleArn { get; set; }

    [CommandSwitch("--component-role-arn")]
    public string? ComponentRoleArn { get; set; }

    [CommandSwitch("--role-arn")]
    public string? RoleArn { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}