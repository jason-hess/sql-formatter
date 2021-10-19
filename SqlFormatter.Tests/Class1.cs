using FluentAssertions;
using NUnit.Framework;

namespace SqlFormatter.Tests;

public class Class1
{
    [Test]
    public void Should()
    {
        var sql = @"
CREATE TABLE [dbo].[SqlTable]
(
	[Id] INT NOT NULL PRIMARY KEY
)";

        var formattedSql = new Formatter().Format(sql);

        formattedSql.Last().Should().Be(';');
    }

    [Test]
    public void Should2()
    {
        var sql = @"CREATE TABLE dbo.SqlTable
(
	Id INT NOT NULL PRIMARY KEY
)";

        var formattedSql = new Formatter().Format(sql);

        formattedSql.Should().Be(@"CREATE TABLE dbo.SqlTable
(
	Id INT NOT NULL PRIMARY KEY
);");
    }
}
