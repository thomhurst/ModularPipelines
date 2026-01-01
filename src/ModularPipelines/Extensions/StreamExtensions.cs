namespace ModularPipelines.Extensions;

/// <summary>
/// Extensions for Streams.
/// </summary>
public static class StreamExtensions
{
    /// <summary>
    /// Turns a generic <see cref="Stream"/> into a <see cref="MemoryStream"/>.
    /// </summary>
    /// <param name="stream">Any stream.</param>
    /// <param name="disposeSource">
    /// When true, disposes the source stream after copying.
    /// Defaults to false - callers are responsible for disposing the source stream.
    /// </param>
    /// <returns>A MemoryStream containing the Stream's data, with Position set to 0 for reading.</returns>
    public static async Task<MemoryStream> ToMemoryStreamAsync(this Stream stream, bool disposeSource = false)
    {
        if (stream is MemoryStream memoryStream)
        {
            if (memoryStream.CanSeek)
            {
                memoryStream.Position = 0;
            }

            return memoryStream;
        }

        memoryStream = new MemoryStream();

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