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
    /// <returns>A MemoryStream containing the Stream's data.</returns>
    public static async Task<MemoryStream> ToMemoryStreamAsync(this Stream stream)
    {
        if (stream is MemoryStream memoryStream)
        {
            return memoryStream;
        }

        memoryStream = new MemoryStream();

        if (stream.CanSeek)
        {
            stream.Position = 0;
        }

        await stream.CopyToAsync(memoryStream);

        await stream.DisposeAsync();

        return memoryStream;
    }
}
