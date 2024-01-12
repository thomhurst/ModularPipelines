using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iam", "workforce-pools", "providers", "create-saml")]
public record GcloudIamWorkforcePoolsProvidersCreateSamlOptions(
[property: PositionalArgument] string Provider,
[property: PositionalArgument] string Location,
[property: PositionalArgument] string WorkforcePool,
[property: CommandSwitch("--attribute-mapping")] IEnumerable<KeyValue> AttributeMapping,
[property: CommandSwitch("--idp-metadata-path")] string IdpMetadataPath
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--attribute-condition")]
    public string? AttributeCondition { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [BooleanCommandSwitch("--disabled")]
    public bool? Disabled { get; set; }

    [CommandSwitch("--display-name")]
    public string? DisplayName { get; set; }
}