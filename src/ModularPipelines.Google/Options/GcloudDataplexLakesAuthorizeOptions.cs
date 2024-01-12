using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dataplex", "lakes", "authorize")]
public record GcloudDataplexLakesAuthorizeOptions(
[property: CommandSwitch("--project-resource")] string ProjectResource,
[property: CommandSwitch("--storage-bucket-resource")] string StorageBucketResource,
[property: CommandSwitch("--bigquery-dataset-resource")] string BigqueryDatasetResource,
[property: CommandSwitch("--secondary-project")] string SecondaryProject
) : GcloudOptions;