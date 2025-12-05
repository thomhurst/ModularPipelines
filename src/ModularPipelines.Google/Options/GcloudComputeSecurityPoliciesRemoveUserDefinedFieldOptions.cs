using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "security-policies", "remove-user-defined-field")]
public record GcloudComputeSecurityPoliciesRemoveUserDefinedFieldOptions(
[property: CliArgument] string Name,
[property: CliOption("--user-defined-field-name")] string UserDefinedFieldName
) : GcloudOptions
{
    [CliOption("--region")]
    public string? Region { get; set; }
}