using ModularPipelines.Google.Services;

namespace ModularPipelines.Google;

internal class Google : IGoogle
{
    public Google(Gcloud gcloud)
    {
        Gcloud = gcloud;
    }

    public Gcloud Gcloud { get; }
}