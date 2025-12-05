using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "interconnects", "attachments", "list")]
public record GcloudComputeInterconnectsAttachmentsListOptions : GcloudOptions;