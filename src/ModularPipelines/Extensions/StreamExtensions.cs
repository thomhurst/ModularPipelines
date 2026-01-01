namespace ModularPipelines.Extensions;

/// <summary>
/// Extensions for Streams.
/// </summary>
public static class StreamExtensions
{
    /// <summary>
    /// Turns a generic <see cref="Stream"/> into a <see cref="MemoryStream"/>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// <b>Breaking change:</b> This method previously always disposed the source stream.
    /// It now defaults to <c>disposeSource: false</c> to follow the principle of least surprise.
    /// Existing callers that relied on automatic disposal should pass <c>disposeSource: true</c>.
    /// </para>
    /// <para>
    /// When the input is already a <see cref="MemoryStream"/> and <paramref name="disposeSource"/> is false,
    /// the same instance is returned (with position reset) as an optimization.
    /// When <paramref name="disposeSource"/> is true, a copy is made before disposing the source.
    /// </para>
    /// <para>
    /// Callers are responsible for disposing the returned <see cref="MemoryStream"/>.
    /// </para>
    /// </remarks>
    /// <param name="stream">Any stream.</param>
    /// <param name="disposeSource">
    /// When true, disposes the source stream after copying.
    /// Defaults to false - callers are responsible for disposing the source stream.
    /// </param>
    /// <returns>A MemoryStream containing the Stream's data, with Position set to 0 for reading.</returns>
    public static async Task<MemoryStream> ToMemoryStreamAsync(this Stream stream, bool disposeSource = false)
    {
        // If input is already a MemoryStream and we don't need to dispose it,
        // return it directly (optimization to avoid unnecessary copy)
        if (stream is MemoryStream sourceMs && !disposeSource)
        {
            if (sourceMs.CanSeek)
            {
                sourceMs.Position = 0;
            }

            return sourceMs;
        }

        // Copy to new MemoryStream (handles both non-MemoryStream inputs
        // and MemoryStream inputs when disposeSource is true)
        var memoryStream = new MemoryStream();

        if (stream.CanSeek)
        {
            stream.Position = 0;
        }

        await stream.CopyToAsync(memoryStream).ConfigureAwait(false);

        if (disposeSource)
        {
            await stream.DisposeAsync().ConfigureAwait(false);
        }

        memoryStream.Position = 0;
        return memoryStream;
    }
}