using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dataplex", "tasks", "create")]
public record GcloudDataplexTasksCreateOptions(
[property: CliArgument] string Task,
[property: CliArgument] string Lake,
[property: CliArgument] string Location,
[property: CliOption("--execution-service-account")] string ExecutionServiceAccount,
[property: CliOption("--execution-args")] IEnumerable<KeyValue> ExecutionArgs,
[property: CliOption("--execution-project")] string ExecutionProject,
[property: CliOption("--kms-key")] string KmsKey,
[property: CliOption("--max-job-execution-lifetime")] string MaxJobExecutionLifetime,
[property: CliOption("--notebook")] string Notebook,
[property: CliOption("--notebook-archive-uris")] string[] NotebookArchiveUris,
[property: CliOption("--notebook-file-uris")] string[] NotebookFileUris,
[property: CliOption("--notebook-batch-executors-count")] string NotebookBatchExecutorsCount,
[property: CliOption("--notebook-batch-max-executors-count")] string NotebookBatchMaxExecutorsCount,
[property: CliOption("--notebook-container-image")] string NotebookContainerImage,
[property: CliOption("--notebook-container-image-java-jars")] string[] NotebookContainerImageJavaJars,
[property: CliOption("--notebook-container-image-properties")] IEnumerable<KeyValue> NotebookContainerImageProperties,
[property: CliOption("--notebook-vpc-network-tags")] string[] NotebookVpcNetworkTags,
[property: CliOption("--notebook-vpc-network-name")] string NotebookVpcNetworkName,
[property: CliOption("--notebook-vpc-sub-network-name")] string NotebookVpcSubNetworkName,
[property: CliOption("--spark-archive-uris")] string[] SparkArchiveUris,
[property: CliOption("--spark-file-uris")] string[] SparkFileUris,
[property: CliOption("--spark-main-class")] string SparkMainClass,
[property: CliOption("--spark-main-jar-file-uri")] string SparkMainJarFileUri,
[property: CliOption("--spark-python-script-file")] string SparkPythonScriptFile,
[property: CliOption("--spark-sql-script")] string SparkSqlScript,
[property: CliOption("--spark-sql-script-file")] string SparkSqlScriptFile,
[property: CliOption("--batch-executors-count")] string BatchExecutorsCount,
[property: CliOption("--batch-max-executors-count")] string BatchMaxExecutorsCount,
[property: CliOption("--container-image")] string ContainerImage,
[property: CliOption("--container-image-java-jars")] string[] ContainerImageJavaJars,
[property: CliOption("--container-image-properties")] IEnumerable<KeyValue> ContainerImageProperties,
[property: CliOption("--container-image-python-packages")] string[] ContainerImagePythonPackages,
[property: CliOption("--vpc-network-tags")] string[] VpcNetworkTags,
[property: CliOption("--vpc-network-name")] string VpcNetworkName,
[property: CliOption("--vpc-sub-network-name")] string VpcSubNetworkName,
[property: CliOption("--trigger-type")] string TriggerType,
[property: CliFlag("on-demand")] bool OnDemand,
[property: CliFlag("recurring")] bool Recurring,
[property: CliFlag("--trigger-disabled")] bool TriggerDisabled,
[property: CliOption("--trigger-max-retires")] string TriggerMaxRetires,
[property: CliOption("--trigger-schedule")] string TriggerSchedule,
[property: CliOption("--trigger-start-time")] string TriggerStartTime
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--display-name")]
    public string? DisplayName { get; set; }

    [CliOption("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }
}