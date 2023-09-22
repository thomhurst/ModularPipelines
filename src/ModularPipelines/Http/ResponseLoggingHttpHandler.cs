using ModularPipelines.Logging;

namespace ModularPipelines.Http;

public class ResponseLoggingHttpHandler : DelegatingHandler
{
    private readonly IModuleLoggerProvider _moduleLoggerProvider;

    public ResponseLoggingHttpHandler(IModuleLoggerProvider moduleLoggerProvider)
    {
        _moduleLoggerProvider = moduleLoggerProvider;
    }
    
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var logger = _moduleLoggerProvider.GetLogger();
        
        var response = await base.SendAsync(request, cancellationToken);
        
        await HttpLogger.PrintResponse(response, logger);
        
        return response.EnsureSuccessStatusCode();
    }
}