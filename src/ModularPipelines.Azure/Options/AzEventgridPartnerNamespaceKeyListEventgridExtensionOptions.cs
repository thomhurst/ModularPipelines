using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eventgrid", "partner", "namespace", "key", "list", "(eventgrid", "extension)")]
public record AzEventgridPartnerNamespaceKeyListEventgridExtensionOptions(
[property: CommandSwitch("--partner-namespace-name")] string PartnerNamespaceName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions;