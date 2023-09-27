namespace ModularPipelines.Extensions;

public static class StreamExtensions
{
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