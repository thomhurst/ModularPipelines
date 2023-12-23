using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("proton", "create-environment")]
public record AwsProtonCreateEnvironmentOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--spec")] string Spec,
[property: CommandSwitch("--template-major-version")] string TemplateMajorVersion,
[property: CommandSwitch("--template-name")] string TemplateName
) : AwsOptions
{
    [CommandSwitch("--codebuild-role-arn")]
    public string? CodebuildRoleArn { get; set; }

    [CommandSwitch("--component-role-arn")]
    public string? ComponentRoleArn { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--environment-account-connection-id")]
    public string? EnvironmentAccountConnectionId { get; set; }

    [CommandSwitch("--proton-service-role-arn")]
    public string? ProtonServiceRoleArn { get; set; }

    [CommandSwitch("--provisioning-repository")]
    public string? ProvisioningRepository { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--template-minor-version")]
    public string? TemplateMinorVersion { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}