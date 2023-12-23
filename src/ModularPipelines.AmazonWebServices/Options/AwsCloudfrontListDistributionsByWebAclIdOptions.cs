using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloudfront", "list-distributions-by-web-acl-id")]
public record AwsCloudfrontListDistributionsByWebAclIdOptions(
[property: CommandSwitch("--web-acl-id")] string WebAclId
) : AwsOptions
{
    [CommandSwitch("--marker")]
    public string? Marker { get; set; }

    [CommandSwitch("--max-items")]
    public string? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}