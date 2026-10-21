using ModularPipelines.Exceptions;
using ModularPipelines.Options;

namespace ModularPipelines.Http;

/// <summary>
/// Extension methods for <see cref="HttpResponseMessage"/> handling.
/// </summary>
internal static class HttpResponseExtensions
{
    /// <summary>
    /// Ensures the response has a success status code, throwing a detailed exception if not.
    /// Unlike <see cref="HttpResponseMessage.EnsureSuccessStatusCode"/>, this method captures
    /// a bounded, unredacted response preview in <see cref="PipelineHttpResponseException.ResponseContent"/>
    /// for programmatic inspection. The preview is excluded from the exception message.
    /// </summary>
    /// <param name="response">The HTTP response to check.</param>
    /// <param name="cancellationToken">Cancellation token for reading response content.</param>
    /// <returns>The original response if successful.</returns>
    /// <exception cref="PipelineHttpResponseException">Thrown when the response status code indicates failure.</exception>
    public static Task<HttpResponseMessage> EnsureSuccessStatusCodeWithContentAsync(
        this HttpResponseMessage response,
        CancellationToken cancellationToken = default)
    {
        return response.EnsureSuccessStatusCodeWithContentAsync(HttpLoggingOptions.Default, cancellationToken);
    }

    /// <summary>
    /// Ensures success, applying the logging options to URI redaction in the failure message.
    /// </summary>
    public static async Task<HttpResponseMessage> EnsureSuccessStatusCodeWithContentAsync(
        this HttpResponseMessage response,
        HttpLoggingOptions? loggingOptions,
        CancellationToken cancellationToken = default)
    {
        if (response.IsSuccessStatusCode)
        {
            return response;
        }

        string? responseContent = null;
        try
        {
            const int maximumPreviewBytes = 2000;
            var (preview, isTruncated, replayContent, _) = await HttpContentPreviewReader
                .ReadAsync(response.Content, maximumPreviewBytes, cancellationToken)
                .ConfigureAwait(false);
            response.Content = replayContent;
            responseContent = isTruncated ? preview + "... (truncated)" : preview;
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception ex) when (ex is not (OutOfMemoryException or StackOverflowException))
        {
            cancellationToken.ThrowIfCancellationRequested();

            // Errors reading content do not hide the primary HTTP status failure.
            // The primary error information is in the status code and reason phrase
        }

        cancellationToken.ThrowIfCancellationRequested();

        throw new PipelineHttpResponseException(
            response.StatusCode,
            response.ReasonPhrase,
            responseContent,
            response.RequestMessage?.RequestUri,
            null,
            loggingOptions ?? HttpLoggingOptions.Default);
    }
}
