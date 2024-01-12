using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "commitments", "create-license")]
public record GcloudComputeCommitmentsCreateLicenseOptions(
[property: PositionalArgument] string Commitment,
[property: CommandSwitch("--amount")] string Amount,
[property: CommandSwitch("--license")] string License,
[property: CommandSwitch("--plan")] string Plan
) : GcloudOptions
{
    [CommandSwitch("--cores-per-license")]
    public string? CoresPerLicense { get; set; }

    [CommandSwitch("--region")]
    public string? Region { get; set; }
}