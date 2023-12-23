using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("serverlessrepo", "create-cloud-formation-change-set")]
public record AwsServerlessrepoCreateCloudFormationChangeSetOptions(
[property: CommandSwitch("--application-id")] string ApplicationId,
[property: CommandSwitch("--stack-name")] string StackName
) : AwsOptions
{
    [CommandSwitch("--capabilities")]
    public string[]? Capabilities { get; set; }

    [CommandSwitch("--change-set-name")]
    public string? ChangeSetName { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--notification-arns")]
    public string[]? NotificationArns { get; set; }

    [CommandSwitch("--parameter-overrides")]
    public string[]? ParameterOverrides { get; set; }

    [CommandSwitch("--resource-types")]
    public string[]? ResourceTypes { get; set; }

    [CommandSwitch("--rollback-configuration")]
    public string? RollbackConfiguration { get; set; }

    [CommandSwitch("--semantic-version")]
    public string? SemanticVersion { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--template-id")]
    public string? TemplateId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}