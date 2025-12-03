using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iam", "workforce-pools", "providers", "create-saml")]
public record GcloudIamWorkforcePoolsProvidersCreateSamlOptions(
[property: CliArgument] string Provider,
[property: CliArgument] string Location,
[property: CliArgument] string WorkforcePool,
[property: CliOption("--attribute-mapping")] IEnumerable<KeyValue> AttributeMapping,
[property: CliOption("--idp-metadata-path")] string IdpMetadataPath
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--attribute-condition")]
    public string? AttributeCondition { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliFlag("--disabled")]
    public bool? Disabled { get; set; }

    [CliOption("--display-name")]
    public string? DisplayName { get; set; }
}