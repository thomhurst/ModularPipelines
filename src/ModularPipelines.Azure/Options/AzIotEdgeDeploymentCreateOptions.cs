using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "edge", "deployment", "create")]
public record AzIotEdgeDeploymentCreateOptions(
[property: CliOption("--content")] string Content,
[property: CliOption("--deployment-id")] string DeploymentId
) : AzOptions
{
    [CliOption("--auth-type")]
    public string? AuthType { get; set; }

    [CliOption("--cl")]
    public string? Cl { get; set; }

    [CliOption("--cmq")]
    public string? Cmq { get; set; }

    [CliOption("--hub-name")]
    public string? HubName { get; set; }

    [CliOption("--lab")]
    public string? Lab { get; set; }

    [CliFlag("--layered")]
    public bool? Layered { get; set; }

    [CliOption("--login")]
    public string? Login { get; set; }

    [CliOption("--metrics")]
    public string? Metrics { get; set; }

    [CliFlag("--no-validation")]
    public bool? NoValidation { get; set; }

    [CliOption("--pri")]
    public string? Pri { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--target-condition")]
    public string? TargetCondition { get; set; }
}