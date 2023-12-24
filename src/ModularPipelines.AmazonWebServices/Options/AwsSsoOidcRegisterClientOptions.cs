using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sso-oidc", "register-client")]
public record AwsSsoOidcRegisterClientOptions(
[property: CommandSwitch("--client-name")] string ClientName,
[property: CommandSwitch("--client-type")] string ClientType
) : AwsOptions
{
    [CommandSwitch("--scopes")]
    public string[]? Scopes { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}