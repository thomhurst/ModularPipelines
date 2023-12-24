using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workspaces", "modify-certificate-based-auth-properties")]
public record AwsWorkspacesModifyCertificateBasedAuthPropertiesOptions(
[property: CommandSwitch("--resource-id")] string ResourceId
) : AwsOptions
{
    [CommandSwitch("--certificate-based-auth-properties")]
    public string? CertificateBasedAuthProperties { get; set; }

    [CommandSwitch("--properties-to-delete")]
    public string[]? PropertiesToDelete { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}