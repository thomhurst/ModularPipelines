using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eventgrid", "partner", "namespace", "key", "regenerate", "(eventgrid", "extension)")]
public record AzEventgridPartnerNamespaceKeyRegenerateEventgridExtensionOptions(
[property: CliOption("--key-name")] string KeyName,
[property: CliOption("--partner-namespace-name")] string PartnerNamespaceName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;