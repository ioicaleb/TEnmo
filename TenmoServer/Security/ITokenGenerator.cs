namespace TenmoServer.Security
{
    public interface ITokenGenerator
    {
        /// <summary>
        /// Generates a new authentication token.
        /// </summary>
        /// <param name="username">The user's username</param>
        /// <returns></returns>
        string GenerateToken(int userId, string username);
    }
}
