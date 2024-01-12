using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dataplex", "tasks", "update")]
public record GcloudDataplexTasksUpdateOptions(
[property: PositionalArgument] string Task,
[property: PositionalArgument] string Lake,
[property: PositionalArgument] string Location
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--display-name")]
    public string? DisplayName { get; set; }

    [CommandSwitch("--update-labels")]
    public IEnumerable<KeyValue>? UpdateLabels { get; set; }

    [BooleanCommandSwitch("--clear-labels")]
    public bool? ClearLabels { get; set; }

    [CommandSwitch("--remove-labels")]
    public string[]? RemoveLabels { get; set; }

    [CommandSwitch("--execution-args")]
    public IEnumerable<KeyValue>? ExecutionArgs { get; set; }

    [CommandSwitch("--execution-project")]
    public string? ExecutionProject { get; set; }

    [CommandSwitch("--execution-service-account")]
    public string? ExecutionServiceAccount { get; set; }

    [CommandSwitch("--kms-key")]
    public string? KmsKey { get; set; }

    [CommandSwitch("--max-job-execution-lifetime")]
    public string? MaxJobExecutionLifetime { get; set; }

    [CommandSwitch("--notebook")]
    public string? Notebook { get; set; }

    [CommandSwitch("--notebook-archive-uris")]
    public string[]? NotebookArchiveUris { get; set; }

    [CommandSwitch("--notebook-file-uris")]
    public string[]? NotebookFileUris { get; set; }

    [CommandSwitch("--notebook-batch-executors-count")]
    public string? NotebookBatchExecutorsCount { get; set; }

    [CommandSwitch("--notebook-batch-max-executors-count")]
    public string? NotebookBatchMaxExecutorsCount { get; set; }

    [CommandSwitch("--notebook-container-image")]
    public string? NotebookContainerImage { get; set; }

    [CommandSwitch("--notebook-container-image-java-jars")]
    public string[]? NotebookContainerImageJavaJars { get; set; }

    [CommandSwitch("--notebook-container-image-properties")]
    public IEnumerable<KeyValue>? NotebookContainerImageProperties { get; set; }

    [CommandSwitch("--notebook-vpc-network-tags")]
    public string[]? NotebookVpcNetworkTags { get; set; }

    [CommandSwitch("--notebook-vpc-network-name")]
    public string? NotebookVpcNetworkName { get; set; }

    [CommandSwitch("--notebook-vpc-sub-network-name")]
    public string? NotebookVpcSubNetworkName { get; set; }

    [CommandSwitch("--spark-archive-uris")]
    public string[]? SparkArchiveUris { get; set; }

    [CommandSwitch("--spark-file-uris")]
    public string[]? SparkFileUris { get; set; }

    [CommandSwitch("--batch-executors-count")]
    public string? BatchExecutorsCount { get; set; }

    [CommandSwitch("--batch-max-executors-count")]
    public string? BatchMaxExecutorsCount { get; set; }

    [CommandSwitch("--container-image")]
    public string? ContainerImage { get; set; }

    [CommandSwitch("--container-image-java-jars")]
    public string[]? ContainerImageJavaJars { get; set; }

    [CommandSwitch("--container-image-properties")]
    public IEnumerable<KeyValue>? ContainerImageProperties { get; set; }

    [CommandSwitch("--container-image-python-packages")]
    public string[]? ContainerImagePythonPackages { get; set; }

    [CommandSwitch("--vpc-network-tags")]
    public string[]? VpcNetworkTags { get; set; }

    [CommandSwitch("--vpc-network-name")]
    public string? VpcNetworkName { get; set; }

    [CommandSwitch("--vpc-sub-network-name")]
    public string? VpcSubNetworkName { get; set; }

    [CommandSwitch("--spark-main-class")]
    public string? SparkMainClass { get; set; }

    [CommandSwitch("--spark-main-jar-file-uri")]
    public string? SparkMainJarFileUri { get; set; }

    [CommandSwitch("--spark-python-script-file")]
    public string? SparkPythonScriptFile { get; set; }

    [CommandSwitch("--spark-sql-script")]
    public string? SparkSqlScript { get; set; }

    [CommandSwitch("--spark-sql-script-file")]
    public string? SparkSqlScriptFile { get; set; }

    [BooleanCommandSwitch("--trigger-disabled")]
    public bool? TriggerDisabled { get; set; }

    [CommandSwitch("--trigger-max-retires")]
    public string? TriggerMaxRetires { get; set; }

    [CommandSwitch("--trigger-schedule")]
    public string? TriggerSchedule { get; set; }

    [CommandSwitch("--trigger-start-time")]
    public string? TriggerStartTime { get; set; }
}