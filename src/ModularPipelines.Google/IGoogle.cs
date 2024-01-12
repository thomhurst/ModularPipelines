using ModularPipelines.Google.Services;

namespace ModularPipelines.Google;

public interface IGoogle
{
    Gcloud Gcloud { get; }
}