using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("security", "regulatory-compliance-standards", "list")]
public record AzSecurityRegulatoryComplianceStandardsListOptions : AzOptions;