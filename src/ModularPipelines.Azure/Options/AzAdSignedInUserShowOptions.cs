using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("ad", "signed-in-user", "show")]
public record AzAdSignedInUserShowOptions : AzOptions;