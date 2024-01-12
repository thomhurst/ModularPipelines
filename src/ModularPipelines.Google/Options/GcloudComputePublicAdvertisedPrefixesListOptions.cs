using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "public-advertised-prefixes", "list")]
public record GcloudComputePublicAdvertisedPrefixesListOptions : GcloudOptions;