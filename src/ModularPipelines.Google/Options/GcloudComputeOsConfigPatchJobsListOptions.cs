using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "os-config", "patch-jobs", "list")]
public record GcloudComputeOsConfigPatchJobsListOptions : GcloudOptions;