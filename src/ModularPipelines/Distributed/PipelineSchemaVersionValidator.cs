namespace ModularPipelines.Distributed;

internal static class PipelineSchemaVersionValidator
{
    public static void Validate(string localVersion, string remoteVersion, string remoteRole)
    {
        if (string.Equals(localVersion, remoteVersion, StringComparison.Ordinal))
        {
            return;
        }

        throw new PipelineSchemaMismatchException(
            $"Distributed pipeline schema mismatch: local schema '{localVersion}' does not match " +
            $"{remoteRole} schema '{remoteVersion}'. Ensure every distributed process runs the same pipeline definition.");
    }
}

internal sealed class PipelineSchemaMismatchException(string message) : InvalidOperationException(message);
