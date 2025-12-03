using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workspaces", "modify-certificate-based-auth-properties")]
public record AwsWorkspacesModifyCertificateBasedAuthPropertiesOptions(
[property: CliOption("--resource-id")] string ResourceId
) : AwsOptions
{
    [CliOption("--certificate-based-auth-properties")]
    public string? CertificateBasedAuthProperties { get; set; }

    [CliOption("--properties-to-delete")]
    public string[]? PropertiesToDelete { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}