using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("identity", "list-operations")]
public record AzIdentityListOperationsOptions : AzOptions;