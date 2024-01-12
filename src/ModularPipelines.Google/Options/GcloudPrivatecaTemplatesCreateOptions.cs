using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("privateca", "templates", "create")]
public record GcloudPrivatecaTemplatesCreateOptions(
[property: PositionalArgument] string CertificateTemplate,
[property: PositionalArgument] string Location,
[property: BooleanCommandSwitch("--copy-sans")] bool CopySans,
[property: BooleanCommandSwitch("--copy-subject")] bool CopySubject
) : GcloudOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--identity-cel-expression")]
    public string? IdentityCelExpression { get; set; }

    [CommandSwitch("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CommandSwitch("--predefined-values-file")]
    public string? PredefinedValuesFile { get; set; }

    [BooleanCommandSwitch("--copy-all-requested-extensions")]
    public bool? CopyAllRequestedExtensions { get; set; }

    [CommandSwitch("--copy-extensions-by-oid")]
    public string[]? CopyExtensionsByOid { get; set; }

    [CommandSwitch("--copy-known-extensions")]
    public string[]? CopyKnownExtensions { get; set; }
}