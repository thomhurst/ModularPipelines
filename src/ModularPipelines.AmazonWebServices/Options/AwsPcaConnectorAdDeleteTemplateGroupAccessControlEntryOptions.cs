using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pca-connector-ad", "delete-template-group-access-control-entry")]
public record AwsPcaConnectorAdDeleteTemplateGroupAccessControlEntryOptions(
[property: CliOption("--group-security-identifier")] string GroupSecurityIdentifier,
[property: CliOption("--template-arn")] string TemplateArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}