using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("privateca", "templates", "create")]
public record GcloudPrivatecaTemplatesCreateOptions(
[property: CliArgument] string CertificateTemplate,
[property: CliArgument] string Location,
[property: CliFlag("--copy-sans")] bool CopySans,
[property: CliFlag("--copy-subject")] bool CopySubject
) : GcloudOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--identity-cel-expression")]
    public string? IdentityCelExpression { get; set; }

    [CliOption("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CliOption("--predefined-values-file")]
    public string? PredefinedValuesFile { get; set; }

    [CliFlag("--copy-all-requested-extensions")]
    public bool? CopyAllRequestedExtensions { get; set; }

    [CliOption("--copy-extensions-by-oid")]
    public string[]? CopyExtensionsByOid { get; set; }

    [CliOption("--copy-known-extensions")]
    public string[]? CopyKnownExtensions { get; set; }
}