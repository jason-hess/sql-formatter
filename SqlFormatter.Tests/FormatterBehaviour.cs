using FluentAssertions;
using NUnit.Framework;

namespace SqlFormatter.Tests
{
    internal class FormatterBehaviour
    {
        [Test]
        public void Should()
        {
            var query = @"
insert into tblThing( ColumnOne ) values( 1 )
select * from tblTable
-- comment
GO";
            var formatter = new Formatter();
            var formattedSql = new Formatter().Format(query);
            formattedSql.Should().Be(@"
insert into tblThing( ColumnOne ) values( 1 );
select * from tblTable;
-- comment
GO");
        }

        [Test]
        public void Should2()
        {
            var formatter = new Formatter();
            var files = Directory.GetFiles(@"C:\Users\aujasonh\Source\MichaelHill\OperationalDataStore\ODS", "*.sql", new EnumerationOptions() { RecurseSubdirectories = true });
            foreach (var file in files)
            {
                var content = File.ReadAllText(file);
                if (content.Length > 122761) continue;
                var formattedContent = new Formatter().Format(content);
                if (content != formattedContent)
                {
                    File.WriteAllText(file, formattedContent);
                }
            }
        }
    }
}
