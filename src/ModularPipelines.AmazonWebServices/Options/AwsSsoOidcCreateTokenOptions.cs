using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sso-oidc", "create-token")]
public record AwsSsoOidcCreateTokenOptions(
[property: CommandSwitch("--client-id")] string ClientId,
[property: CommandSwitch("--client-secret")] string ClientSecret,
[property: CommandSwitch("--grant-type")] string GrantType
) : AwsOptions
{
    [CommandSwitch("--device-code")]
    public string? DeviceCode { get; set; }

    [CommandSwitch("--code")]
    public string? Code { get; set; }

    [CommandSwitch("--refresh-token")]
    public string? RefreshToken { get; set; }

    [CommandSwitch("--scope")]
    public string[]? Scope { get; set; }

    [CommandSwitch("--redirect-uri")]
    public string? RedirectUri { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}