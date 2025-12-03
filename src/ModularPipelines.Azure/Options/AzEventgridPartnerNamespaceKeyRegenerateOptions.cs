using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("eventgrid", "partner", "namespace", "key", "regenerate")]
public record AzEventgridPartnerNamespaceKeyRegenerateOptions(
[property: CliOption("--key-name")] string KeyName,
[property: CliOption("--partner-namespace-name")] string PartnerNamespaceName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;