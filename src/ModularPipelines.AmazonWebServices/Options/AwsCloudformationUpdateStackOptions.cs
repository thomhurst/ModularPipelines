using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudformation", "update-stack")]
public record AwsCloudformationUpdateStackOptions(
[property: CliOption("--stack-name")] string StackName
) : AwsOptions
{
    [CliOption("--template-body")]
    public string? TemplateBody { get; set; }

    [CliOption("--template-url")]
    public string? TemplateUrl { get; set; }

    [CliOption("--stack-policy-during-update-body")]
    public string? StackPolicyDuringUpdateBody { get; set; }

    [CliOption("--stack-policy-during-update-url")]
    public string? StackPolicyDuringUpdateUrl { get; set; }

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

    [CliOption("--stack-policy-body")]
    public string? StackPolicyBody { get; set; }

    [CliOption("--stack-policy-url")]
    public string? StackPolicyUrl { get; set; }

    [CliOption("--notification-arns")]
    public string[]? NotificationArns { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}