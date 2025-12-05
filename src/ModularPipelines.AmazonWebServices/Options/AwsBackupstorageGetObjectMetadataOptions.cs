using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("backupstorage", "get-object-metadata")]
public record AwsBackupstorageGetObjectMetadataOptions(
[property: CliOption("--storage-job-id")] string StorageJobId,
[property: CliOption("--object-token")] string ObjectToken
) : AwsOptions;