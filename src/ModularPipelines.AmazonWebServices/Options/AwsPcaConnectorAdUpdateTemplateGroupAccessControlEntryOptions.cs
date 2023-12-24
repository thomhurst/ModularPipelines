using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pca-connector-ad", "update-template-group-access-control-entry")]
public record AwsPcaConnectorAdUpdateTemplateGroupAccessControlEntryOptions(
[property: CommandSwitch("--group-security-identifier")] string GroupSecurityIdentifier,
[property: CommandSwitch("--template-arn")] string TemplateArn
) : AwsOptions
{
    [CommandSwitch("--access-rights")]
    public string? AccessRights { get; set; }

    [CommandSwitch("--group-display-name")]
    public string? GroupDisplayName { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}