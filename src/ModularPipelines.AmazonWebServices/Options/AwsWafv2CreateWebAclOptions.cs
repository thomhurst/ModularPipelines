using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("wafv2", "create-web-acl")]
public record AwsWafv2CreateWebAclOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--scope")] string Scope,
[property: CommandSwitch("--default-action")] string DefaultAction,
[property: CommandSwitch("--visibility-config")] string VisibilityConfig
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--rules")]
    public string[]? Rules { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--custom-response-bodies")]
    public IEnumerable<KeyValue>? CustomResponseBodies { get; set; }

    [CommandSwitch("--captcha-config")]
    public string? CaptchaConfig { get; set; }

    [CommandSwitch("--challenge-config")]
    public string? ChallengeConfig { get; set; }

    [CommandSwitch("--token-domains")]
    public string[]? TokenDomains { get; set; }

    [CommandSwitch("--association-config")]
    public string? AssociationConfig { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}