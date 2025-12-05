using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("domains", "list-user-verified")]
public record GcloudDomainsListUserVerifiedOptions : GcloudOptions;