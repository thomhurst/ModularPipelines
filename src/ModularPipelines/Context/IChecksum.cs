namespace ModularPipelines.Context;

public interface IChecksum
{
    string Md5(string filePath);
}