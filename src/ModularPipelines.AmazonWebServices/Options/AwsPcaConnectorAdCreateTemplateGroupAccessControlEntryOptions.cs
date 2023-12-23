using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pca-connector-ad", "create-template-group-access-control-entry")]
public record AwsPcaConnectorAdCreateTemplateGroupAccessControlEntryOptions(
[property: CommandSwitch("--access-rights")] string AccessRights,
[property: CommandSwitch("--group-display-name")] string GroupDisplayName,
[property: CommandSwitch("--group-security-identifier")] string GroupSecurityIdentifier,
[property: CommandSwitch("--template-arn")] string TemplateArn
) : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}