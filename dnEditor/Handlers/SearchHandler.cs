using System.Windows.Forms;

namespace dnEditor.Handlers
{
    public static class SearchHandler
    {
        public enum SearchType
        {
            Any,
            Type,
            Method,
            Field,
            String,
            OpCode,
            Operand,
        }

        public static TreeNode DoSearch(TreeNode node, string text, SearchType searchType)
        {
            switch (searchType)
            {
                case SearchType.Type: //TODO: Implement
                case SearchType.Method: //TODO: Implement
                case SearchType.Field: //TODO: Implement
                case SearchType.Any:
                    if (node.Text.Contains(text))
                    {
                        return node;
                    }
                    break;

                case SearchType.OpCode: //TODO: Implement
                case SearchType.String: //TODO: Implement
                case SearchType.Operand: //TODO: Implement
                    break;
            }

            return null;
        }
    }
}