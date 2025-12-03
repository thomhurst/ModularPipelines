using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "security-policies", "add-user-defined-field")]
public record GcloudComputeSecurityPoliciesAddUserDefinedFieldOptions(
[property: CliArgument] string Name,
[property: CliOption("--base")] string Base,
[property: CliOption("--offset")] string Offset,
[property: CliOption("--size")] string Size,
[property: CliOption("--user-defined-field-name")] string UserDefinedFieldName
) : GcloudOptions
{
    [CliOption("--mask")]
    public string? Mask { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }
}