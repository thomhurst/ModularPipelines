using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("privateca", "templates", "update")]
public record GcloudPrivatecaTemplatesUpdateOptions(
[property: CliArgument] string CertificateTemplate,
[property: CliArgument] string Location
) : GcloudOptions
{
    [CliFlag("--copy-sans")]
    public bool? CopySans { get; set; }

    [CliFlag("--copy-subject")]
    public bool? CopySubject { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--identity-cel-expression")]
    public string? IdentityCelExpression { get; set; }

    [CliOption("--predefined-values-file")]
    public string? PredefinedValuesFile { get; set; }

    [CliOption("--update-labels")]
    public IEnumerable<KeyValue>? UpdateLabels { get; set; }

    [CliFlag("--clear-labels")]
    public bool? ClearLabels { get; set; }

    [CliOption("--remove-labels")]
    public string[]? RemoveLabels { get; set; }

    [CliFlag("--copy-all-requested-extensions")]
    public bool? CopyAllRequestedExtensions { get; set; }

    [CliOption("--copy-extensions-by-oid")]
    public string[]? CopyExtensionsByOid { get; set; }

    [CliFlag("--drop-oid-extensions")]
    public bool? DropOidExtensions { get; set; }

    [CliOption("--copy-known-extensions")]
    public string[]? CopyKnownExtensions { get; set; }

    [CliFlag("--drop-known-extensions")]
    public bool? DropKnownExtensions { get; set; }
}