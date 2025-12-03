using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dataplex", "lakes", "authorize")]
public record GcloudDataplexLakesAuthorizeOptions(
[property: CliOption("--project-resource")] string ProjectResource,
[property: CliOption("--storage-bucket-resource")] string StorageBucketResource,
[property: CliOption("--bigquery-dataset-resource")] string BigqueryDatasetResource,
[property: CliOption("--secondary-project")] string SecondaryProject
) : GcloudOptions;