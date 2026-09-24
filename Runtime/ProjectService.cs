using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase.Firestore;

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
    /// Hard-references Firebase.Firestore (installed as a loose DLL under Assets/Firebase, not a
    /// UPM package -- so there's no version-defines mechanism to guard this optionally).
    /// </summary>
    public class ProjectService
    {
        public async Task<string> CreateProjectAsync(string ownerId, string name)
        {
            var docRef = FirebaseFirestore.DefaultInstance.Collection("interiors").Document();

            var data = new Dictionary<string, object>
            {
                { "ownerId", ownerId },
                { "name", name },
                { "createdAt", FieldValue.ServerTimestamp }
            };

            await docRef.SetAsync(data);
            return docRef.Id;
        }

        public async Task<List<ProjectSummary>> ListMyProjectsAsync(string ownerId)
        {
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
        }
    }
}
