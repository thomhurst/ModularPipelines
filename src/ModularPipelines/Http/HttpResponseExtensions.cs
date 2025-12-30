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
        catch
        {
            // If we can't read the content, continue with null
        }

        throw new HttpResponseException(
            response.StatusCode,
            response.ReasonPhrase,
            responseContent,
            response.RequestMessage?.RequestUri);
    }
}
