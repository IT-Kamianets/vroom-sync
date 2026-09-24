using System.Threading.Tasks;
using Firebase.Auth;

namespace VRoom.Sync
{
    /// <summary>
    /// Email/password auth via Firebase Auth. Deliberately just sign up / sign in / sign out /
    /// current user id -- no password reset, email verification, or other providers yet.
    ///
    /// Hard-references Firebase.Auth (installed as a loose DLL under Assets/Firebase, not a UPM
    /// package -- so there's no version-defines mechanism to guard this optionally). Not verified
    /// against a live installed SDK version in the Editor yet.
    /// </summary>
    public class AuthService
    {
        public string CurrentUserId => FirebaseAuth.DefaultInstance.CurrentUser?.UserId;

        public async Task<string> SignUpAsync(string email, string password)
        {
            var result = await FirebaseAuth.DefaultInstance.CreateUserWithEmailAndPasswordAsync(email, password);
            return result.User.UserId;
        }

        public async Task<string> SignInAsync(string email, string password)
        {
            var result = await FirebaseAuth.DefaultInstance.SignInWithEmailAndPasswordAsync(email, password);
            return result.User.UserId;
        }

        public void SignOut()
        {
            FirebaseAuth.DefaultInstance.SignOut();
        }
    }
}
