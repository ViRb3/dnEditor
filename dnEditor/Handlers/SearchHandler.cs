using System;
using System.Windows.Forms;
using dnlib.DotNet;

namespace dnEditor.Handlers
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

    public class SearchHandler
    {
        public delegate void EventHandler(TreeNode foundNode);

        private readonly SearchType _searchType;
        private readonly string _text;

        public TreeNode FoundNode = null;
        private TreeNode _searchNode;

        public SearchHandler(TreeNode searchNode, string text, SearchType searchType)
        {
            if (!(searchNode.Tag is ModuleDefMD))
                throw new ArgumentException("TreeNode must be ModuleDefMD!");

            _searchNode = searchNode;
            _text = text;
            _searchType = searchType;
        }

        public event EventHandler SearchFinished;

        public static TreeNode Search(TreeNode node, string text, SearchType searchType)
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