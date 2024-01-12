using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dataplex", "tasks", "create")]
public record GcloudDataplexTasksCreateOptions(
[property: PositionalArgument] string Task,
[property: PositionalArgument] string Lake,
[property: PositionalArgument] string Location,
[property: CommandSwitch("--execution-service-account")] string ExecutionServiceAccount,
[property: CommandSwitch("--execution-args")] IEnumerable<KeyValue> ExecutionArgs,
[property: CommandSwitch("--execution-project")] string ExecutionProject,
[property: CommandSwitch("--kms-key")] string KmsKey,
[property: CommandSwitch("--max-job-execution-lifetime")] string MaxJobExecutionLifetime,
[property: CommandSwitch("--notebook")] string Notebook,
[property: CommandSwitch("--notebook-archive-uris")] string[] NotebookArchiveUris,
[property: CommandSwitch("--notebook-file-uris")] string[] NotebookFileUris,
[property: CommandSwitch("--notebook-batch-executors-count")] string NotebookBatchExecutorsCount,
[property: CommandSwitch("--notebook-batch-max-executors-count")] string NotebookBatchMaxExecutorsCount,
[property: CommandSwitch("--notebook-container-image")] string NotebookContainerImage,
[property: CommandSwitch("--notebook-container-image-java-jars")] string[] NotebookContainerImageJavaJars,
[property: CommandSwitch("--notebook-container-image-properties")] IEnumerable<KeyValue> NotebookContainerImageProperties,
[property: CommandSwitch("--notebook-vpc-network-tags")] string[] NotebookVpcNetworkTags,
[property: CommandSwitch("--notebook-vpc-network-name")] string NotebookVpcNetworkName,
[property: CommandSwitch("--notebook-vpc-sub-network-name")] string NotebookVpcSubNetworkName,
[property: CommandSwitch("--spark-archive-uris")] string[] SparkArchiveUris,
[property: CommandSwitch("--spark-file-uris")] string[] SparkFileUris,
[property: CommandSwitch("--spark-main-class")] string SparkMainClass,
[property: CommandSwitch("--spark-main-jar-file-uri")] string SparkMainJarFileUri,
[property: CommandSwitch("--spark-python-script-file")] string SparkPythonScriptFile,
[property: CommandSwitch("--spark-sql-script")] string SparkSqlScript,
[property: CommandSwitch("--spark-sql-script-file")] string SparkSqlScriptFile,
[property: CommandSwitch("--batch-executors-count")] string BatchExecutorsCount,
[property: CommandSwitch("--batch-max-executors-count")] string BatchMaxExecutorsCount,
[property: CommandSwitch("--container-image")] string ContainerImage,
[property: CommandSwitch("--container-image-java-jars")] string[] ContainerImageJavaJars,
[property: CommandSwitch("--container-image-properties")] IEnumerable<KeyValue> ContainerImageProperties,
[property: CommandSwitch("--container-image-python-packages")] string[] ContainerImagePythonPackages,
[property: CommandSwitch("--vpc-network-tags")] string[] VpcNetworkTags,
[property: CommandSwitch("--vpc-network-name")] string VpcNetworkName,
[property: CommandSwitch("--vpc-sub-network-name")] string VpcSubNetworkName,
[property: CommandSwitch("--trigger-type")] string TriggerType,
[property: BooleanCommandSwitch("on-demand")] bool OnDemand,
[property: BooleanCommandSwitch("recurring")] bool Recurring,
[property: BooleanCommandSwitch("--trigger-disabled")] bool TriggerDisabled,
[property: CommandSwitch("--trigger-max-retires")] string TriggerMaxRetires,
[property: CommandSwitch("--trigger-schedule")] string TriggerSchedule,
[property: CommandSwitch("--trigger-start-time")] string TriggerStartTime
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--display-name")]
    public string? DisplayName { get; set; }

    [CommandSwitch("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }
}