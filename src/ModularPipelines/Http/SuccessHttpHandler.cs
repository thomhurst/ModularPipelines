namespace ModularPipelines.Http;

public class SuccessHttpHandler : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var response = await base.SendAsync(request, cancellationToken);
        return response.EnsureSuccessStatusCode();
    }
}