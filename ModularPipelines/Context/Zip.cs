using System.IO.Compression;
using ModularPipelines.FileSystem;

namespace ModularPipelines.Context;

public class Zip : IZip
{
    public void ZipFolder( Folder folder, string outputPath, CompressionLevel compressionLevel )
    {
        ZipFile.CreateFromDirectory( folder.Path, outputPath, compressionLevel, false );
    }

    public void UnZipToFolder( string zipPath, string outputFolderPath )
    {
        ZipFile.ExtractToDirectory( zipPath, outputFolderPath, true );
    }
}
