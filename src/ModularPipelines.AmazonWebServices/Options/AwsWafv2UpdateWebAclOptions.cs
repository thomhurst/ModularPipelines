using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("wafv2", "update-web-acl")]
public record AwsWafv2UpdateWebAclOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--scope")] string Scope,
[property: CliOption("--id")] string Id,
[property: CliOption("--default-action")] string DefaultAction,
[property: CliOption("--visibility-config")] string VisibilityConfig,
[property: CliOption("--lock-token")] string LockToken
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--rules")]
    public string[]? Rules { get; set; }

    [CliOption("--custom-response-bodies")]
    public IEnumerable<KeyValue>? CustomResponseBodies { get; set; }

    [CliOption("--captcha-config")]
    public string? CaptchaConfig { get; set; }

    [CliOption("--challenge-config")]
    public string? ChallengeConfig { get; set; }

    [CliOption("--token-domains")]
    public string[]? TokenDomains { get; set; }

    [CliOption("--association-config")]
    public string? AssociationConfig { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}