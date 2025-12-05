using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("serverlessrepo", "create-cloud-formation-change-set")]
public record AwsServerlessrepoCreateCloudFormationChangeSetOptions(
[property: CliOption("--application-id")] string ApplicationId,
[property: CliOption("--stack-name")] string StackName
) : AwsOptions
{
    [CliOption("--capabilities")]
    public string[]? Capabilities { get; set; }

    [CliOption("--change-set-name")]
    public string? ChangeSetName { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--notification-arns")]
    public string[]? NotificationArns { get; set; }

    [CliOption("--parameter-overrides")]
    public string[]? ParameterOverrides { get; set; }

    [CliOption("--resource-types")]
    public string[]? ResourceTypes { get; set; }

    [CliOption("--rollback-configuration")]
    public string? RollbackConfiguration { get; set; }

    [CliOption("--semantic-version")]
    public string? SemanticVersion { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--template-id")]
    public string? TemplateId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}