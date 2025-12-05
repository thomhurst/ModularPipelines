using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("staticwebapp", "identity", "show")]
public record AzStaticwebappIdentityShowOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;