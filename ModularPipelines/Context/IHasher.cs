namespace ModularPipelines.Context;

public interface IHasher
{
    string Sha1( string input, HashType hashType = HashType.Hex );
    string Sha256( string input, HashType hashType = HashType.Hex );
    string Sha384( string input, HashType hashType = HashType.Hex );
    string Sha512( string input, HashType hashType = HashType.Hex );
    string Md5( string input, HashType hashType = HashType.Hex );
}
