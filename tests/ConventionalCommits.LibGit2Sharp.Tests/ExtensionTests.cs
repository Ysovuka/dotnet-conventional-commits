namespace ConventionalCommits.LibGit2Sharp.Tests
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using global::LibGit2Sharp;
    using Xunit;

    public sealed class ExtensionTests
    {
        [Theory]
        [InlineData("https://github.com/Ysovuka/dotnet-conventional-commits.git", "test-tag", "Equals")]
        [InlineData("https://github.com/Ysovuka/dotnet-conventional-commits.git", "test", "NotEquals")]
        [InlineData("https://github.com/Ysovuka/dotnet-conventional-commits.git", "test", "Contains")]
        [InlineData("https://github.com/Ysovuka/dotnet-conventional-commits.git", "test", "StartsWith")]
        [InlineData("https://github.com/Ysovuka/dotnet-conventional-commits.git", "tag", "EndsWith")]
        public async Task GetCommitMessagesFromTagAsync_ReturnsAnyMessages(string gitUrl, string tag, string @operator)
        {
            // Given
            var tempDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            var path = Repository.Clone(gitUrl, tempDirectory);

            var filterCriteriaOperator = (FilterCriteriaOperators)Enum.Parse(typeof(FilterCriteriaOperators), @operator);
            var fromCriteria = new FilterCriteria(tag, filterCriteriaOperator);
            var filter = new TagFilter(fromCriteria);

            using var repository = new Repository(path);

            // When
            var commitMessages = await repository.GetCommitMessagesFromTagAsync(filter);

            // Then
            Assert.NotEmpty(commitMessages);
        }
    }
}
