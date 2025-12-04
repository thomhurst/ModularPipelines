using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("load", "test-run", "create")]
public record AzLoadTestRunCreateOptions(
[property: CliOption("--load-test-resource")] string LoadTestResource,
[property: CliOption("--test-id")] string TestId,
[property: CliOption("--test-run-id")] string TestRunId
) : AzOptions
{
    [CliOption("--certificate")]
    public string? Certificate { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--display-name")]
    public string? DisplayName { get; set; }

    [CliOption("--env")]
    public string? Env { get; set; }

    [CliOption("--existing-test-run-id")]
    public string? ExistingTestRunId { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--secret")]
    public string? Secret { get; set; }
}