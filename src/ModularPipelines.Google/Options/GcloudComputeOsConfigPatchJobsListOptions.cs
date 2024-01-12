using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "os-config", "patch-jobs", "list")]
public record GcloudComputeOsConfigPatchJobsListOptions : GcloudOptions;