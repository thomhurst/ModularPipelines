using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iam", "workload-identity-pools", "providers", "create-saml")]
public record GcloudIamWorkloadIdentityPoolsProvidersCreateSamlOptions(
[property: PositionalArgument] string Provider,
[property: PositionalArgument] string Location,
[property: PositionalArgument] string WorkloadIdentityPool,
[property: CommandSwitch("--attribute-mapping")] IEnumerable<KeyValue> AttributeMapping,
[property: CommandSwitch("--idp-metadata-path")] string IdpMetadataPath
) : GcloudOptions
{
    [CommandSwitch("--attribute-condition")]
    public string? AttributeCondition { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [BooleanCommandSwitch("--disabled")]
    public bool? Disabled { get; set; }

    [CommandSwitch("--display-name")]
    public string? DisplayName { get; set; }
}