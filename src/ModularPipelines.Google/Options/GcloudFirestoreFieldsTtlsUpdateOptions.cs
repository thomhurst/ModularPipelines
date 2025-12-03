using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("firestore", "fields", "ttls", "update")]
public record GcloudFirestoreFieldsTtlsUpdateOptions(
[property: CliArgument] string Field,
[property: CliArgument] string CollectionGroup,
[property: CliArgument] string Database,
[property: CliFlag("--disable-ttl")] bool DisableTtl,
[property: CliFlag("--enable-ttl")] bool EnableTtl
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }
}