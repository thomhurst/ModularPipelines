using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("services", "api-keys", "undelete")]
public record GcloudServicesApiKeysUndeleteOptions(
[property: PositionalArgument] string Key,
[property: PositionalArgument] string Location,
[property: PositionalArgument] string KeyString
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }
}