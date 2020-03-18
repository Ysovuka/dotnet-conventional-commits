namespace LibGit2Sharp
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using ConventionalCommits.LibGit2Sharp;

    public static class IRepositoryExtensions
    {
        /// <summary>
        /// Returns a list of commit messages based on commit filter.
        /// </summary>
        /// <param name="repository">Repository to obtain the commit messages.</param>
        /// <param name="commitFilter">Commit filter to be used as query criteria.</param>
        /// <returns>A list of commit messages.</returns>
        private static Task<IEnumerable<string>> GetCommitMessagesByFilterAsync(
            this IRepository repository,
            CommitFilter commitFilter)
        {
            IEnumerable<Commit> commits = repository.Commits.QueryBy(commitFilter);

            var remote = repository.Network.Remotes.FirstOrDefault();
            return Task.FromResult(commits.Select(
                    x =>
                    {
                        var url = string.Empty;

                        if (remote is object)
                        {
                            if (remote.Url.IndexOf("@") > 0)
                            {
                                url = $"{remote.Url.Substring(0, remote.Url.IndexOf("//") + 2)}{remote.Url.Substring(remote.Url.IndexOf("@") + 1)}";
                            }
                            else
                            {
                                url = remote.Url;
                            }
                        }

                        var commitSha = $"\nSha: {x.Sha}";
                        var commitAuthor = $"Author Name: {x.Author.Name}";
                        var commitAuthorEmail = $"Author Email: {x.Author.Email}";
                        var commitDate = $"Date: {x.Author.When}";
                        var commitUrl = $"Url: {url}/commit/{x.Sha}";

                        return string.Join("\n",
                            x.Message,
                            commitSha,
                            commitAuthor,
                            commitAuthorEmail,
                            commitDate,
                            commitUrl);
                    }));
        }

        public static async Task<IEnumerable<string>> GetCommitMessagesFromTagAsync(
            this IRepository repository,
            TagFilter filter)
        {
            var tag = repository.Tags.From(filter);
            if (tag is null)
            {
                throw new NullReferenceException("No tag was found with provided name.");
            }

            var commit = repository.Commits.FirstOrDefault(x => x.Sha.Equals(tag.PeeledTarget.Sha));
            if (commit is null)
            {
                throw new NullReferenceException("No commits were found from provided tag.");
            }

            var exclusionList = new List<Commit>(commit.Parents) { commit };
            var commitFilter = new CommitFilter()
            {
                ExcludeReachableFrom = exclusionList
            };

            return await repository.GetCommitMessagesByFilterAsync(commitFilter);
        }
    }
}
