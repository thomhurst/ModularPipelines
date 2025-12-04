using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("load", "test", "create")]
public record AzLoadTestCreateOptions(
[property: CliOption("--load-test-resource")] string LoadTestResource,
[property: CliOption("--test-id")] string TestId
) : AzOptions
{
    [CliOption("--certificate")]
    public string? Certificate { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--display-name")]
    public string? DisplayName { get; set; }

    [CliOption("--engine-instances")]
    public string? EngineInstances { get; set; }

    [CliOption("--env")]
    public string? Env { get; set; }

    [CliOption("--keyvault-reference-id")]
    public string? KeyvaultReferenceId { get; set; }

    [CliOption("--load-test-config-file")]
    public string? LoadTestConfigFile { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--secret")]
    public string? Secret { get; set; }

    [CliOption("--split-csv")]
    public string? SplitCsv { get; set; }

    [CliOption("--subnet-id")]
    public string? SubnetId { get; set; }

    [CliOption("--test-plan")]
    public string? TestPlan { get; set; }
}