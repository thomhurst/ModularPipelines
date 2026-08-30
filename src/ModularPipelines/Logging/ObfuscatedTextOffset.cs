namespace ModularPipelines.Logging;

internal static class ObfuscatedTextOffset
{
    internal static int Scale(int sourceOffset, int sourceLength, string output)
    {
        if (sourceOffset >= sourceLength)
        {
            return output.Length;
        }

        var scaledOffset = (long) sourceOffset * output.Length / sourceLength;
        var outputOffset = (int) scaledOffset;
        return outputOffset > 0
               && outputOffset < output.Length
               && char.IsLowSurrogate(output[outputOffset])
            ? outputOffset + 1
            : outputOffset;
    }
}
