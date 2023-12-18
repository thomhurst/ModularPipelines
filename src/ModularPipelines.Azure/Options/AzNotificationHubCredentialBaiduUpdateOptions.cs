using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("notification-hub", "credential", "baidu", "update")]
public record AzNotificationHubCredentialBaiduUpdateOptions(
[property: CommandSwitch("--api-key")] string ApiKey,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--namespace-name")] string NamespaceName,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--secret-key")] string SecretKey
) : AzOptions
{
    [CommandSwitch("--baidu-api-key")]
    public string? BaiduApiKey { get; set; }

    [CommandSwitch("--baidu-secret-key")]
    public string? BaiduSecretKey { get; set; }
}

