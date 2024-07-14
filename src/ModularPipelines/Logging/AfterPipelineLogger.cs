using Microsoft.Extensions.Logging;

namespace ModularPipelines.Logging;

internal class AfterPipelineLogger : IAfterPipelineLogger
{
    private readonly ILogger<AfterPipelineLogger> _logger;
    private readonly List<string> _values = [];

    public AfterPipelineLogger(ILogger<AfterPipelineLogger> logger)
    {
        _logger = logger;
    }
    
    public void LogOnPipelineEnd(string value)
    {
        _values.Add(value);
    }

    public string GetOutput()
    {
        return string.Join(Environment.NewLine, _values);
    }

    public void WriteLogs()
    {
        foreach (var value in _values)
        {
            _logger.LogInformation("{Value}", value);
        }
    }
}