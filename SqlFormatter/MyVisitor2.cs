using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace SqlFormatter.Tests
{
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
