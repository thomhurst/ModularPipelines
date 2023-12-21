using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
public class AzApic
{
    public AzApic(
        AzApicApi api,
        AzApicEnvironment environment,
        AzApicMetadataSchema metadataSchema,
        AzApicService service,
        AzApicWorkspace workspace
    )
    {
        Api = api;
        Environment = environment;
        MetadataSchema = metadataSchema;
        Service = service;
        Workspace = workspace;
    }

    public AzApicApi Api { get; }

    public AzApicEnvironment Environment { get; }

    public AzApicMetadataSchema MetadataSchema { get; }

    public AzApicService Service { get; }

    public AzApicWorkspace Workspace { get; }
}