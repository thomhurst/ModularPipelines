using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("waf", "delete-web-acl")]
public record AwsWafDeleteWebAclOptions(
[property: CliOption("--web-acl-id")] string WebAclId,
[property: CliOption("--change-token")] string ChangeToken
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}