using FluentAssertions;
using Microsoft.SqlServer.TransactSql.ScriptDom;
using NUnit.Framework;
using System.Text;

namespace SqlFormatter.Tests
{
    public class Formatter
    {
        public string Format(string sql)
        {
            var formattedChange = false;
            do
            {
                formattedChange = false;
                var visitor = new MyVisitor(sql);
                var parser = new TSql150Parser(false);
                IList<ParseError> errors;
                using var input = new StringReader(sql);
                var parsedQuery = parser.Parse(input, out errors);

                if (errors.Count == 0) parsedQuery.Accept(visitor);

                if (visitor.FormattedChange)
                {
                    sql = visitor.FormattedSql;
                    formattedChange = true;
                }

            } while (formattedChange);

            return sql;
        }
    }

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
            var formattedSql = formatter.Format(query);
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
                var formattedContent = formatter.Format(content);
                if (content != formattedContent)
                {
                    File.WriteAllText(file, formattedContent);
                }
            }
        }

        [Test]
        public void Should3()
        {
            var parser = new TSql150Parser(false);
            IList<ParseError> errors;
            using var input = new StringReader("SELECT /** Test **/ * FROM tblSale");
            var parsedQuery = parser.Parse(input, out errors);
            if(errors.Count == 0)
            {
                parsedQuery.Accept(new MyVisitor2());
            }
            
        }
    }

    class MyVisitor : TSqlFragmentVisitor
    {
        private readonly StringBuilder _query;

        public MyVisitor(string query) => _query = new StringBuilder(query);

        public bool FormattedChange { get; internal set; }
        public string FormattedSql { get; internal set; }

        public override void Visit(TSqlStatement node)
        {
            if (FormattedChange) return;

            var selectStatement = node as SelectStatement;
            if (node != null)
            {
                var lastToken = node.ScriptTokenStream[node.LastTokenIndex];
                if (lastToken.TokenType != TSqlTokenType.Semicolon)
                {
                    var indexOfMissingSemicolon = lastToken.Offset + lastToken.Text.Length;
                    //if(indexOfMissingSemicolon == _query.Length || _query[indexOfMissingSemicolon] != ';')
                    {
                        FormattedSql = _query.Insert(indexOfMissingSemicolon, ";").ToString();
                        FormattedChange = true;
                    }
                }
            }

            base.Visit(node);
        }

        internal void Reset()
        {
            FormattedChange = false;
        }
    }

    class MyVisitor2 : TSqlFragmentVisitor
    {
        public override void Visit(TSqlStatement node)
        {
            var selectStatement = node as SelectStatement;
            if (node != null)
            {
                node.ScriptTokenStream.RemoveAt(0);
                var lastToken = node.ScriptTokenStream[node.LastTokenIndex];
                if (lastToken.TokenType != TSqlTokenType.Semicolon)
                {
                    //var indexOfMissingSemicolon = lastToken.Offset + lastToken.Text.Length;
                }
            }

            base.Visit(node);
        }
    }
}
