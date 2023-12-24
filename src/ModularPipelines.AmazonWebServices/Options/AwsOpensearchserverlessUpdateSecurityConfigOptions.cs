using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("opensearchserverless", "update-security-config")]
public record AwsOpensearchserverlessUpdateSecurityConfigOptions(
[property: CommandSwitch("--config-version")] string ConfigVersion,
[property: CommandSwitch("--id")] string Id
) : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--saml-options")]
    public string? SamlOptions { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}