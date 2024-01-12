using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datastore", "operations", "list")]
public record GcloudDatastoreOperationsListOptions : GcloudOptions;