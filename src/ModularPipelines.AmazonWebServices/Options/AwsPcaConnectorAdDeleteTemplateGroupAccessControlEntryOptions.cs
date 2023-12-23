using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pca-connector-ad", "delete-template-group-access-control-entry")]
public record AwsPcaConnectorAdDeleteTemplateGroupAccessControlEntryOptions(
[property: CommandSwitch("--group-security-identifier")] string GroupSecurityIdentifier,
[property: CommandSwitch("--template-arn")] string TemplateArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}