using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloudformation", "create-stack")]
public record AwsCloudformationCreateStackOptions(
[property: CommandSwitch("--stack-name")] string StackName
) : AwsOptions
{
    [CommandSwitch("--template-body")]
    public string? TemplateBody { get; set; }

    [CommandSwitch("--template-url")]
    public string? TemplateUrl { get; set; }

    [CommandSwitch("--parameters")]
    public string[]? Parameters { get; set; }

    [CommandSwitch("--rollback-configuration")]
    public string? RollbackConfiguration { get; set; }

    [CommandSwitch("--timeout-in-minutes")]
    public int? TimeoutInMinutes { get; set; }

    [CommandSwitch("--notification-arns")]
    public string[]? NotificationArns { get; set; }

    [CommandSwitch("--capabilities")]
    public string[]? Capabilities { get; set; }

    [CommandSwitch("--resource-types")]
    public string[]? ResourceTypes { get; set; }

    [CommandSwitch("--role-arn")]
    public string? RoleArn { get; set; }

    [CommandSwitch("--on-failure")]
    public string? OnFailure { get; set; }

    [CommandSwitch("--stack-policy-body")]
    public string? StackPolicyBody { get; set; }

    [CommandSwitch("--stack-policy-url")]
    public string? StackPolicyUrl { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}