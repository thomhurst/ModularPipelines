using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "ssl-certificates", "create")]
public record GcloudComputeSslCertificatesCreateOptions(
[property: PositionalArgument] string Name,
[property: CommandSwitch("--domains")] string[] Domains,
[property: CommandSwitch("--certificate")] string Certificate,
[property: CommandSwitch("--private-key")] string PrivateKey
) : GcloudOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [BooleanCommandSwitch("--global")]
    public bool? Global { get; set; }

    [CommandSwitch("--region")]
    public string? Region { get; set; }
}