using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eventgrid", "partner", "namespace", "key", "regenerate", "(eventgrid", "extension)")]
public record AzEventgridPartnerNamespaceKeyRegenerateEventgridExtensionOptions(
[property: CommandSwitch("--key-name")] string KeyName,
[property: CommandSwitch("--partner-namespace-name")] string PartnerNamespaceName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions;