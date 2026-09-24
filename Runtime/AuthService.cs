using System.Threading.Tasks;

#if FIREBASE_AUTH_PRESENT
using Firebase.Auth;
#endif

namespace VRoom.Sync
{
    /// <summary>
    /// Email/password auth via Firebase Auth. Deliberately just sign up / sign in / sign out /
    /// current user id -- no password reset, email verification, or other providers yet.
    ///
    /// Same optional-dependency pattern as SceneSyncService: compiles fine without the Firebase
    /// Auth SDK installed, methods just throw if called. Not verified against a live installed
    /// SDK version in the Editor yet.
    /// </summary>
    public class AuthService
    {
        public string CurrentUserId
        {
            get
            {
#if !FIREBASE_AUTH_PRESENT
                throw new System.InvalidOperationException(
                    "Firebase Auth (com.google.firebase.auth) is not installed in this project.");
#else
                return FirebaseAuth.DefaultInstance.CurrentUser?.UserId;
#endif
            }
        }

        public async Task<string> SignUpAsync(string email, string password)
        {
#if !FIREBASE_AUTH_PRESENT
            throw new System.InvalidOperationException(
                "Firebase Auth (com.google.firebase.auth) is not installed in this project.");
#else
            var result = await FirebaseAuth.DefaultInstance.CreateUserWithEmailAndPasswordAsync(email, password);
            return result.User.UserId;
#endif
        }

        public async Task<string> SignInAsync(string email, string password)
        {
#if !FIREBASE_AUTH_PRESENT
            throw new System.InvalidOperationException(
                "Firebase Auth (com.google.firebase.auth) is not installed in this project.");
#else
            var result = await FirebaseAuth.DefaultInstance.SignInWithEmailAndPasswordAsync(email, password);
            return result.User.UserId;
#endif
        }

        public void SignOut()
        {
#if FIREBASE_AUTH_PRESENT
            FirebaseAuth.DefaultInstance.SignOut();
#endif
        }
    }
}
