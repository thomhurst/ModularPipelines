using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workspaces-web", "update-trust-store")]
public record AwsWorkspacesWebUpdateTrustStoreOptions(
[property: CommandSwitch("--trust-store-arn")] string TrustStoreArn
) : AwsOptions
{
    [CommandSwitch("--certificates-to-add")]
    public string[]? CertificatesToAdd { get; set; }

    [CommandSwitch("--certificates-to-delete")]
    public string[]? CertificatesToDelete { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}