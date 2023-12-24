using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workspaces-web", "create-browser-settings")]
public record AwsWorkspacesWebCreateBrowserSettingsOptions(
[property: CommandSwitch("--browser-policy")] string BrowserPolicy
) : AwsOptions
{
    [CommandSwitch("--additional-encryption-context")]
    public IEnumerable<KeyValue>? AdditionalEncryptionContext { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--customer-managed-key")]
    public string? CustomerManagedKey { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}