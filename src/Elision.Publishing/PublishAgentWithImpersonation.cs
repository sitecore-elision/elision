using Sitecore.Diagnostics;
using Sitecore.Security.Accounts;

namespace Elision.Foundation.Publishing
{
    public class PublishAgentWithImpersonation : Sitecore.Tasks.PublishAgent
    {
        public string Username { get; protected set; }

        public PublishAgentWithImpersonation(string sourceDatabase, string targetDatabase, string mode, string languages, string username)
            : base(sourceDatabase, targetDatabase, mode, languages)
        {
            Username = username;
        }

        public new void Run()
        {
            Log.Info(
                $"{this} started (source: {SourceDatabase}, target: {TargetDatabase}, mode: {Mode}, languages: {string.Join(",", Languages)}, user: {Username})",
                this);

            var user = string.IsNullOrWhiteSpace(Username)
                ? User.Current
                : User.FromName(Username, false);

            if (user == null || user == User.Current)
                base.Run();
            else
                using (new UserSwitcher(user))
                    base.Run();
        }
    }
}
