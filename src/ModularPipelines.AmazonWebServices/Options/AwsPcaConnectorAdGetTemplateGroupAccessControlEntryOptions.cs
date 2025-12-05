using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pca-connector-ad", "get-template-group-access-control-entry")]
public record AwsPcaConnectorAdGetTemplateGroupAccessControlEntryOptions(
[property: CliOption("--group-security-identifier")] string GroupSecurityIdentifier,
[property: CliOption("--template-arn")] string TemplateArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}