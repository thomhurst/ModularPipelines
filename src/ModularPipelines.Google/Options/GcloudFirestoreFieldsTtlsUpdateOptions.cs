using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("firestore", "fields", "ttls", "update")]
public record GcloudFirestoreFieldsTtlsUpdateOptions(
[property: PositionalArgument] string Field,
[property: PositionalArgument] string CollectionGroup,
[property: PositionalArgument] string Database,
[property: BooleanCommandSwitch("--disable-ttl")] bool DisableTtl,
[property: BooleanCommandSwitch("--enable-ttl")] bool EnableTtl
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }
}