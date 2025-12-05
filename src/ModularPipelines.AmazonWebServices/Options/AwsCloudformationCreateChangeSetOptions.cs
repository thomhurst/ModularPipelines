using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudformation", "create-change-set")]
public record AwsCloudformationCreateChangeSetOptions(
[property: CliOption("--stack-name")] string StackName,
[property: CliOption("--change-set-name")] string ChangeSetName
) : AwsOptions
{
    [CliOption("--template-body")]
    public string? TemplateBody { get; set; }

    [CliOption("--template-url")]
    public string? TemplateUrl { get; set; }

    [CliOption("--parameters")]
    public string[]? Parameters { get; set; }

    [CliOption("--capabilities")]
    public string[]? Capabilities { get; set; }

    [CliOption("--resource-types")]
    public string[]? ResourceTypes { get; set; }

    [CliOption("--role-arn")]
    public string? RoleArn { get; set; }

    [CliOption("--rollback-configuration")]
    public string? RollbackConfiguration { get; set; }

    [CliOption("--notification-arns")]
    public string[]? NotificationArns { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--change-set-type")]
    public string? ChangeSetType { get; set; }

    [CliOption("--resources-to-import")]
    public string[]? ResourcesToImport { get; set; }

    [CliOption("--on-stack-failure")]
    public string? OnStackFailure { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}