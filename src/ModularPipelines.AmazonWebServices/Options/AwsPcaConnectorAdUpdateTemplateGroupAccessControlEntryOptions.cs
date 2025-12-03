using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pca-connector-ad", "update-template-group-access-control-entry")]
public record AwsPcaConnectorAdUpdateTemplateGroupAccessControlEntryOptions(
[property: CliOption("--group-security-identifier")] string GroupSecurityIdentifier,
[property: CliOption("--template-arn")] string TemplateArn
) : AwsOptions
{
    [CliOption("--access-rights")]
    public string? AccessRights { get; set; }

    [CliOption("--group-display-name")]
    public string? GroupDisplayName { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}