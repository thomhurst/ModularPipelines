using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("opensearchserverless", "update-security-config")]
public record AwsOpensearchserverlessUpdateSecurityConfigOptions(
[property: CliOption("--config-version")] string ConfigVersion,
[property: CliOption("--id")] string Id
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--saml-options")]
    public string? SamlOptions { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}