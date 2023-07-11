namespace ModularPipelines.Context;

public interface IHasher
{
    /// <summary>
    /// Hashes the input to an Sha1 encoded string
    /// </summary>
    /// <param name="input">The input to hash</param>
    /// <param name="hashType">The encoded string output format</param>
    /// <returns></returns>
    string Sha1(string input, HashType hashType = HashType.Hex);

    /// <summary>
    /// Hashes the input to an Sha256 encoded string
    /// </summary>
    /// <param name="input">The input to hash</param>
    /// <param name="hashType">The encoded string output format</param>
    /// <returns></returns>
    string Sha256(string input, HashType hashType = HashType.Hex);

    /// <summary>
    /// Hashes the input to an Sha384 encoded string
    /// </summary>
    /// <param name="input">The input to hash</param>
    /// <param name="hashType">The encoded string output format</param>
    /// <returns></returns>
    string Sha384(string input, HashType hashType = HashType.Hex);

    /// <summary>
    /// Hashes the input to an Sha512 encoded string
    /// </summary>
    /// <param name="input"></param>
    /// <param name="hashType">The encoded string output format</param>
    /// <returns></returns>
    string Sha512(string input, HashType hashType = HashType.Hex);

    /// <summary>
    /// Hashes the input to an md5 encoded string
    /// </summary>
    /// <param name="input">The input to hash</param>
    /// <param name="hashType">The encoded string output format</param>
    /// <returns></returns>
    string Md5(string input, HashType hashType = HashType.Hex);
}
