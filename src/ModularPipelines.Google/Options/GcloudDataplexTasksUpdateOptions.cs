using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dataplex", "tasks", "update")]
public record GcloudDataplexTasksUpdateOptions(
[property: CliArgument] string Task,
[property: CliArgument] string Lake,
[property: CliArgument] string Location
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--display-name")]
    public string? DisplayName { get; set; }

    [CliOption("--update-labels")]
    public IEnumerable<KeyValue>? UpdateLabels { get; set; }

    [CliFlag("--clear-labels")]
    public bool? ClearLabels { get; set; }

    [CliOption("--remove-labels")]
    public string[]? RemoveLabels { get; set; }

    [CliOption("--execution-args")]
    public IEnumerable<KeyValue>? ExecutionArgs { get; set; }

    [CliOption("--execution-project")]
    public string? ExecutionProject { get; set; }

    [CliOption("--execution-service-account")]
    public string? ExecutionServiceAccount { get; set; }

    [CliOption("--kms-key")]
    public string? KmsKey { get; set; }

    [CliOption("--max-job-execution-lifetime")]
    public string? MaxJobExecutionLifetime { get; set; }

    [CliOption("--notebook")]
    public string? Notebook { get; set; }

    [CliOption("--notebook-archive-uris")]
    public string[]? NotebookArchiveUris { get; set; }

    [CliOption("--notebook-file-uris")]
    public string[]? NotebookFileUris { get; set; }

    [CliOption("--notebook-batch-executors-count")]
    public string? NotebookBatchExecutorsCount { get; set; }

    [CliOption("--notebook-batch-max-executors-count")]
    public string? NotebookBatchMaxExecutorsCount { get; set; }

    [CliOption("--notebook-container-image")]
    public string? NotebookContainerImage { get; set; }

    [CliOption("--notebook-container-image-java-jars")]
    public string[]? NotebookContainerImageJavaJars { get; set; }

    [CliOption("--notebook-container-image-properties")]
    public IEnumerable<KeyValue>? NotebookContainerImageProperties { get; set; }

    [CliOption("--notebook-vpc-network-tags")]
    public string[]? NotebookVpcNetworkTags { get; set; }

    [CliOption("--notebook-vpc-network-name")]
    public string? NotebookVpcNetworkName { get; set; }

    [CliOption("--notebook-vpc-sub-network-name")]
    public string? NotebookVpcSubNetworkName { get; set; }

    [CliOption("--spark-archive-uris")]
    public string[]? SparkArchiveUris { get; set; }

    [CliOption("--spark-file-uris")]
    public string[]? SparkFileUris { get; set; }

    [CliOption("--batch-executors-count")]
    public string? BatchExecutorsCount { get; set; }

    [CliOption("--batch-max-executors-count")]
    public string? BatchMaxExecutorsCount { get; set; }

    [CliOption("--container-image")]
    public string? ContainerImage { get; set; }

    [CliOption("--container-image-java-jars")]
    public string[]? ContainerImageJavaJars { get; set; }

    [CliOption("--container-image-properties")]
    public IEnumerable<KeyValue>? ContainerImageProperties { get; set; }

    [CliOption("--container-image-python-packages")]
    public string[]? ContainerImagePythonPackages { get; set; }

    [CliOption("--vpc-network-tags")]
    public string[]? VpcNetworkTags { get; set; }

    [CliOption("--vpc-network-name")]
    public string? VpcNetworkName { get; set; }

    [CliOption("--vpc-sub-network-name")]
    public string? VpcSubNetworkName { get; set; }

    [CliOption("--spark-main-class")]
    public string? SparkMainClass { get; set; }

    [CliOption("--spark-main-jar-file-uri")]
    public string? SparkMainJarFileUri { get; set; }

    [CliOption("--spark-python-script-file")]
    public string? SparkPythonScriptFile { get; set; }

    [CliOption("--spark-sql-script")]
    public string? SparkSqlScript { get; set; }

    [CliOption("--spark-sql-script-file")]
    public string? SparkSqlScriptFile { get; set; }

    [CliFlag("--trigger-disabled")]
    public bool? TriggerDisabled { get; set; }

    [CliOption("--trigger-max-retires")]
    public string? TriggerMaxRetires { get; set; }

    [CliOption("--trigger-schedule")]
    public string? TriggerSchedule { get; set; }

    [CliOption("--trigger-start-time")]
    public string? TriggerStartTime { get; set; }
}