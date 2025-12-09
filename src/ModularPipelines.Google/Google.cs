using ModularPipelines.Google.Services;

namespace ModularPipelines.Google;

internal class Google : IGoogle
{
    public Google(IGcloud gcloud)
    {
        Gcloud = gcloud;
    }

    public IGcloud Gcloud { get; }
}