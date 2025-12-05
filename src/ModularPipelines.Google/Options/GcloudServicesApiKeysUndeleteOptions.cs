using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("services", "api-keys", "undelete")]
public record GcloudServicesApiKeysUndeleteOptions(
[property: CliArgument] string Key,
[property: CliArgument] string Location,
[property: CliArgument] string KeyString
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }
}