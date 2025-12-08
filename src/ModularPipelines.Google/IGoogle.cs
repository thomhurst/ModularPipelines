using ModularPipelines.Google.Services;

namespace ModularPipelines.Google;

public interface IGoogle
{
    IGcloud Gcloud { get; }
}