namespace ConventionalCommits.LibGit2Sharp
{
    using System.Collections.Generic;
    using System.Linq;
    using global::LibGit2Sharp;

    public sealed class TagFilter
    {
        public TagFilter(FilterCriteria fromCriteria) => From = fromCriteria;

        public FilterCriteria From { get; }
    }

    public sealed class FilterCriteria
    {
        public FilterCriteria(string value, FilterCriteriaOperators @operator)
        {
            Value = value;
            Operator = @operator;
        }

        public string Value { get; }
        public FilterCriteriaOperators Operator { get; }

        public IEnumerable<Tag> Apply(TagCollection tagCollection)
        {
            var tags = tagCollection.AsEnumerable();

            switch (Operator)
            {
                case FilterCriteriaOperators.Equals:
                    tags = tags.Where(x => x.FriendlyName.Equals(Value));
                    break;

                case FilterCriteriaOperators.NotEquals:
                    tags = tags.Where(x => !x.FriendlyName.Equals(Value));
                    break;

                case FilterCriteriaOperators.Contains:
                    tags = tags.Where(x => x.FriendlyName.Contains(Value));
                    break;
                case FilterCriteriaOperators.StartsWith:
                    tags = tags.Where(x => x.FriendlyName.StartsWith(Value));
                    break;
                case FilterCriteriaOperators.EndsWith:
                    tags = tags.Where(x => x.FriendlyName.EndsWith(Value));
                    break;
            }

            return tags;
        }
    }

    public enum FilterCriteriaOperators : byte
    {
        Equals = 1,
        NotEquals = 2,
        Contains = 3,
        StartsWith = 8,
        EndsWith = 15
    }
}
