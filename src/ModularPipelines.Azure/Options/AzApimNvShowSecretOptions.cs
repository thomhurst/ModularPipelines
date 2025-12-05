using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("apim", "nv", "show-secret")]
public record AzApimNvShowSecretOptions(
[property: CliOption("--named-value-id")] string NamedValueId,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--service-name")] string ServiceName
) : AzOptions;