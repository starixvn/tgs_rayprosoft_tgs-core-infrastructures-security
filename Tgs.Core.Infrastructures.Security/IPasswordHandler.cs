namespace Tgs.Core.Infrastructures.Security
{
    public interface IPasswordHandler
    {
        (byte[] Salt, byte[] HashedPassword) GenerateCredential(string password);

        bool Verify(byte[] existedPassword, byte[] existedSalt, string password);

        byte[] GenerateRandomSalt(ushort length);
    }
}