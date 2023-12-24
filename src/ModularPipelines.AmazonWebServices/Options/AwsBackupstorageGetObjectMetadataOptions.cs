using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("backupstorage", "get-object-metadata")]
public record AwsBackupstorageGetObjectMetadataOptions(
[property: CommandSwitch("--storage-job-id")] string StorageJobId,
[property: CommandSwitch("--object-token")] string ObjectToken
) : AwsOptions;