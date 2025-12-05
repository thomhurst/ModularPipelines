using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("waf", "update-web-acl")]
public record AwsWafUpdateWebAclOptions(
[property: CliOption("--web-acl-id")] string WebAclId,
[property: CliOption("--change-token")] string ChangeToken
) : AwsOptions
{
    [CliOption("--updates")]
    public string[]? Updates { get; set; }

    [CliOption("--default-action")]
    public string? DefaultAction { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}