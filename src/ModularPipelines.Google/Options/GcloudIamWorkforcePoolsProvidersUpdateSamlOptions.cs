using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iam", "workforce-pools", "providers", "update-saml")]
public record GcloudIamWorkforcePoolsProvidersUpdateSamlOptions(
[property: PositionalArgument] string Provider,
[property: PositionalArgument] string Location,
[property: PositionalArgument] string WorkforcePool
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--attribute-condition")]
    public string? AttributeCondition { get; set; }

    [CommandSwitch("--attribute-mapping")]
    public IEnumerable<KeyValue>? AttributeMapping { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--display-name")]
    public string? DisplayName { get; set; }

    [CommandSwitch("--idp-metadata-path")]
    public string? IdpMetadataPath { get; set; }
}