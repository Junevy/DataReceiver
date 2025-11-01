using FubarDev.FtpServer.AccountManagement;
using System.Security.Claims;

namespace DataReceiver.Models.Socket.FTP
{
    /// <summary>
    /// 用于验证FTP 登录权限。RootPath <see cref="FtpServerConfig"/>
    /// </summary>
    /// <param name="userName">公共用户名</param>
    /// <param name="password">公共密码</param>
    public class UserShipProvider(string userName, string password) : IMembershipProvider
    {
        // private readonly Dictionary<string, string> UserShip = [];
        private readonly string publicUserName = userName;
        private readonly string publicPassword = password;

        public Task<MemberValidationResult> ValidateUserAsync(string username, string password)
        {
            if (this.publicUserName.Equals(username) && this.publicPassword.Equals(password))
            {
                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, "public user"),
                    new Claim(ClaimTypes.Role, "public"),
                };
                var identity = new ClaimsIdentity(claims, "public user");
                var user = new ClaimsPrincipal(identity);

                return Task.FromResult(new MemberValidationResult(MemberValidationStatus.AuthenticatedUser, user));
            }

            return Task.FromResult(new MemberValidationResult(MemberValidationStatus.InvalidLogin));
        }
    }
}
