using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eventarc", "locations", "list")]
public record GcloudEventarcLocationsListOptions : GcloudOptions;