using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("proton", "create-environment")]
public record AwsProtonCreateEnvironmentOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--spec")] string Spec,
[property: CliOption("--template-major-version")] string TemplateMajorVersion,
[property: CliOption("--template-name")] string TemplateName
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

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--template-minor-version")]
    public string? TemplateMinorVersion { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}