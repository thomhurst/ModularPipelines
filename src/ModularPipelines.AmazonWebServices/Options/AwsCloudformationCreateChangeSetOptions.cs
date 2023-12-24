using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloudformation", "create-change-set")]
public record AwsCloudformationCreateChangeSetOptions(
[property: CommandSwitch("--stack-name")] string StackName,
[property: CommandSwitch("--change-set-name")] string ChangeSetName
) : AwsOptions
{
    [CommandSwitch("--template-body")]
    public string? TemplateBody { get; set; }

    [CommandSwitch("--template-url")]
    public string? TemplateUrl { get; set; }

    [CommandSwitch("--parameters")]
    public string[]? Parameters { get; set; }

    [CommandSwitch("--capabilities")]
    public string[]? Capabilities { get; set; }

    [CommandSwitch("--resource-types")]
    public string[]? ResourceTypes { get; set; }

    [CommandSwitch("--role-arn")]
    public string? RoleArn { get; set; }

    [CommandSwitch("--rollback-configuration")]
    public string? RollbackConfiguration { get; set; }

    [CommandSwitch("--notification-arns")]
    public string[]? NotificationArns { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--change-set-type")]
    public string? ChangeSetType { get; set; }

    [CommandSwitch("--resources-to-import")]
    public string[]? ResourcesToImport { get; set; }

    [CommandSwitch("--on-stack-failure")]
    public string? OnStackFailure { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}