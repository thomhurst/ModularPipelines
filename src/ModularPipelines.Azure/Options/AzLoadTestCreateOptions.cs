using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("load", "test", "create")]
public record AzLoadTestCreateOptions(
[property: CommandSwitch("--load-test-resource")] string LoadTestResource,
[property: CommandSwitch("--test-id")] string TestId
) : AzOptions
{
    [CommandSwitch("--certificate")]
    public string? Certificate { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--display-name")]
    public string? DisplayName { get; set; }

    [CommandSwitch("--engine-instances")]
    public string? EngineInstances { get; set; }

    [CommandSwitch("--env")]
    public string? Env { get; set; }

    [CommandSwitch("--keyvault-reference-id")]
    public string? KeyvaultReferenceId { get; set; }

    [CommandSwitch("--load-test-config-file")]
    public string? LoadTestConfigFile { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--secret")]
    public string? Secret { get; set; }

    [CommandSwitch("--split-csv")]
    public string? SplitCsv { get; set; }

    [CommandSwitch("--subnet-id")]
    public string? SubnetId { get; set; }

    [CommandSwitch("--test-plan")]
    public string? TestPlan { get; set; }
}