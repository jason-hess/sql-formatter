using Microsoft.SqlServer.TransactSql.ScriptDom;
using System.Text;

namespace SqlFormatter.Tests
{
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
}
