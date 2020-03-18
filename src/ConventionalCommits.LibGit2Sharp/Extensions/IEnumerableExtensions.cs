namespace LibGit2Sharp
{
    using System.Linq;
    using ConventionalCommits.LibGit2Sharp;

    public static class IEnumerableExtensions
    {
        public static Tag From(this TagCollection tagCollection,
            TagFilter filter)        
            => filter.From.Apply(tagCollection).FirstOrDefault();
        
    }
}
