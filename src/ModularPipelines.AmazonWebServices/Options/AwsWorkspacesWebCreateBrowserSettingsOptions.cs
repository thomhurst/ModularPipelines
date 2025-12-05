using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workspaces-web", "create-browser-settings")]
public record AwsWorkspacesWebCreateBrowserSettingsOptions(
[property: CliOption("--browser-policy")] string BrowserPolicy
) : AwsOptions
{
    [CliOption("--additional-encryption-context")]
    public IEnumerable<KeyValue>? AdditionalEncryptionContext { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--customer-managed-key")]
    public string? CustomerManagedKey { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}