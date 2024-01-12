using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("domains", "list-user-verified")]
public record GcloudDomainsListUserVerifiedOptions : GcloudOptions;