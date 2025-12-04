using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("notification-hub", "credential", "baidu", "update")]
public record AzNotificationHubCredentialBaiduUpdateOptions(
[property: CliOption("--api-key")] string ApiKey,
[property: CliOption("--name")] string Name,
[property: CliOption("--namespace-name")] string NamespaceName,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--secret-key")] string SecretKey
) : AzOptions
{
    [CliOption("--baidu-api-key")]
    public string? BaiduApiKey { get; set; }

    [CliOption("--baidu-secret-key")]
    public string? BaiduSecretKey { get; set; }
}