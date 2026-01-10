using ModularPipelines.Exceptions;

namespace ModularPipelines.Http;

/// <summary>
/// Extension methods for <see cref="HttpResponseMessage"/> handling.
/// </summary>
internal static class HttpResponseExtensions
{
    /// <summary>
    /// Ensures the response has a success status code, throwing a detailed exception if not.
    /// Unlike <see cref="HttpResponseMessage.EnsureSuccessStatusCode"/>, this method includes
    /// the response content in the exception for easier debugging.
    /// </summary>
    /// <param name="response">The HTTP response to check.</param>
    /// <param name="cancellationToken">Cancellation token for reading response content.</param>
    /// <returns>The original response if successful.</returns>
    /// <exception cref="HttpResponseException">Thrown when the response status code indicates failure.</exception>
    public static async Task<HttpResponseMessage> EnsureSuccessStatusCodeWithContentAsync(
        this HttpResponseMessage response,
        CancellationToken cancellationToken = default)
    {
        if (response.IsSuccessStatusCode)
        {
            return response;
        }

        string? responseContent = null;
        try
        {
            responseContent = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
        }
        catch (ObjectDisposedException)
        {
            // Response content stream was already disposed - continue with null content
        }
        catch (InvalidOperationException)
        {
            // Content stream is in an invalid state - continue with null content
        }
        catch (HttpRequestException)
        {
            // Network error while reading content - continue with null content
        }
        catch (OperationCanceledException)
        {
            // Reading was cancelled - continue with null content
        }
        catch (Exception ex) when (ex is not (OutOfMemoryException or StackOverflowException))
        {
            // Other unexpected errors reading content - continue with null content
            // The primary error information is in the status code and reason phrase
        }

        throw new HttpResponseException(
            response.StatusCode,
            response.ReasonPhrase,
            responseContent,
            response.RequestMessage?.RequestUri);
    }
}
