using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("firestore", "locations", "list")]
public record GcloudFirestoreLocationsListOptions : GcloudOptions;