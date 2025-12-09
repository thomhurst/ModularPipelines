using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("proton", "update-environment")]
public record AwsProtonUpdateEnvironmentOptions(
[property: CliOption("--deployment-type")] string DeploymentType,
[property: CliOption("--name")] string Name
) : AwsOptions
{
    [CliOption("--codebuild-role-arn")]
    public string? CodebuildRoleArn { get; set; }

    [CliOption("--component-role-arn")]
    public string? ComponentRoleArn { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--environment-account-connection-id")]
    public string? EnvironmentAccountConnectionId { get; set; }

    [CliOption("--proton-service-role-arn")]
    public string? ProtonServiceRoleArn { get; set; }

    [CliOption("--provisioning-repository")]
    public string? ProvisioningRepository { get; set; }

    [CliOption("--spec")]
    public string? Spec { get; set; }

    [CliOption("--template-major-version")]
    public string? TemplateMajorVersion { get; set; }

    [CliOption("--template-minor-version")]
    public string? TemplateMinorVersion { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}