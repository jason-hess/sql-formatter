using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace SqlFormatter.Tests
{
    public class Formatter
    {
        public string Format(string sql)
        {
            bool formattedChange;
            do
            {
                formattedChange = false;
                var visitor = new MyVisitor(sql);
                var parser = new TSql150Parser(false);
                using var input = new StringReader(sql);
                var parsedQuery = parser.Parse(input, out IList<ParseError> errors);

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
}
