using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("devops", "user", "update")]
public record AzDevopsUserUpdateOptions(
[property: CliOption("--license-type")] string LicenseType,
[property: CliOption("--user")] string User
) : AzOptions
{
    [CliFlag("--detect")]
    public bool? Detect { get; set; }

    [CliOption("--org")]
    public string? Org { get; set; }
}