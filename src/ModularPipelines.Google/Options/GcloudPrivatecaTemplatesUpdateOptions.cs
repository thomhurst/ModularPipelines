using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("privateca", "templates", "update")]
public record GcloudPrivatecaTemplatesUpdateOptions(
[property: PositionalArgument] string CertificateTemplate,
[property: PositionalArgument] string Location
) : GcloudOptions
{
    [BooleanCommandSwitch("--copy-sans")]
    public bool? CopySans { get; set; }

    [BooleanCommandSwitch("--copy-subject")]
    public bool? CopySubject { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--identity-cel-expression")]
    public string? IdentityCelExpression { get; set; }

    [CommandSwitch("--predefined-values-file")]
    public string? PredefinedValuesFile { get; set; }

    [CommandSwitch("--update-labels")]
    public IEnumerable<KeyValue>? UpdateLabels { get; set; }

    [BooleanCommandSwitch("--clear-labels")]
    public bool? ClearLabels { get; set; }

    [CommandSwitch("--remove-labels")]
    public string[]? RemoveLabels { get; set; }

    [BooleanCommandSwitch("--copy-all-requested-extensions")]
    public bool? CopyAllRequestedExtensions { get; set; }

    [CommandSwitch("--copy-extensions-by-oid")]
    public string[]? CopyExtensionsByOid { get; set; }

    [BooleanCommandSwitch("--drop-oid-extensions")]
    public bool? DropOidExtensions { get; set; }

    [CommandSwitch("--copy-known-extensions")]
    public string[]? CopyKnownExtensions { get; set; }

    [BooleanCommandSwitch("--drop-known-extensions")]
    public bool? DropKnownExtensions { get; set; }
}