using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("services", "api-keys", "list")]
public record GcloudServicesApiKeysListOptions : GcloudOptions
{
    [BooleanCommandSwitch("--show-deleted")]
    public bool? ShowDeleted { get; set; }
}