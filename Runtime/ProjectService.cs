using System.Collections.Generic;
using System.Threading.Tasks;

#if FIRESTORE_PRESENT
using Firebase.Firestore;
#endif

namespace VRoom.Sync
{
    /// <summary>A project the current user owns -- product/UI name for an "interior" (interiorId).</summary>
    public class ProjectSummary
    {
        public string Id;
        public string Name;
        public string OwnerId;
    }

    /// <summary>
    /// Creates/lists projects (interiors/{interiorId} documents). The Firestore security rules
    /// require every interior document to have an ownerId matching the authenticated user, so
    /// CreateProjectAsync is what makes SceneSyncService's writes (under that interior) actually
    /// pass those rules -- without a project having been created first, every scan upload will
    /// be rejected.
    ///
    /// Same optional-dependency pattern as SceneSyncService: compiles fine without the Firebase
    /// Firestore SDK installed, methods just throw if called.
    /// </summary>
    public class ProjectService
    {
        public async Task<string> CreateProjectAsync(string ownerId, string name)
        {
#if !FIRESTORE_PRESENT
            throw new System.InvalidOperationException(
                "Firebase Firestore (com.google.firebase.firestore) is not installed in this project.");
#else
            var docRef = FirebaseFirestore.DefaultInstance.Collection("interiors").Document();

            var data = new Dictionary<string, object>
            {
                { "ownerId", ownerId },
                { "name", name },
                { "createdAt", FieldValue.ServerTimestamp }
            };

            await docRef.SetAsync(data);
            return docRef.Id;
#endif
        }

        public async Task<List<ProjectSummary>> ListMyProjectsAsync(string ownerId)
        {
#if !FIRESTORE_PRESENT
            throw new System.InvalidOperationException(
                "Firebase Firestore (com.google.firebase.firestore) is not installed in this project.");
#else
            var snapshot = await FirebaseFirestore.DefaultInstance
                .Collection("interiors")
                .WhereEqualTo("ownerId", ownerId)
                .GetSnapshotAsync();

            var projects = new List<ProjectSummary>();
            foreach (var doc in snapshot.Documents)
            {
                projects.Add(new ProjectSummary
                {
                    Id = doc.Id,
                    Name = doc.ContainsField("name") ? doc.GetValue<string>("name") : doc.Id,
                    OwnerId = ownerId
                });
            }

            return projects;
#endif
        }
    }
}
