using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pca-connector-ad", "create-template-group-access-control-entry")]
public record AwsPcaConnectorAdCreateTemplateGroupAccessControlEntryOptions(
[property: CliOption("--access-rights")] string AccessRights,
[property: CliOption("--group-display-name")] string GroupDisplayName,
[property: CliOption("--group-security-identifier")] string GroupSecurityIdentifier,
[property: CliOption("--template-arn")] string TemplateArn
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}