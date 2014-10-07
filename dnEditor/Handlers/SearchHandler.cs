using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using dnEditor.Forms;
using dnEditor.Misc;
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
        public delegate void EventHandler(object result);

        private readonly TreeView _searchTreeView;
        private readonly TreeNode _searchNode;

        private readonly SearchType _searchType;
        private readonly string _text;
        private readonly TreeViewHandler _treeViewHandler;

        public TreeNode FoundNode = null;

        public SearchHandler(TreeNode searchNode, string text, SearchType searchType, TreeViewHandler treeViewHandler)
        {
            _searchNode = searchNode;
            _searchTreeView = searchNode.TreeView;

            _text = text.ToLower();
            _searchType = searchType;
            _treeViewHandler = treeViewHandler;
        }

        public SearchHandler(TreeView searchTreeView, string text, SearchType searchType, TreeViewHandler treeViewHandler)
        {
            _searchTreeView = searchTreeView;
            _text = text.ToLower();
            _searchType = searchType;
            _treeViewHandler = treeViewHandler;
        }

        public event EventHandler SearchFinished;

        public void Search()
        {
            object result = DoSearch();
            SearchFinished.Invoke(result);
        }

        private object DoSearch()
        {
            if (_searchType == SearchType.Type) //TODO: Implement
            {
            }
            if (_searchType == SearchType.Method) //TODO: Implement
            {
            }
            if (_searchType == SearchType.Field) //TODO: Implement
            {
            }
            if (_searchType == SearchType.Any) //TODO: Implement
            {
            }

            if (_searchType == SearchType.OpCode)
            {
                #region OpCode

                if (!(_searchNode.Tag is MethodDef)) return null;

                DataGridView dgBody = MainForm.DgBody;

                List<DataGridViewRow> resultRows =
                    dgBody.Rows.Cast<DataGridViewRow>()
                        .Where(r => r.Cells["OpCode"].Value.ToString().ToLower().Trim() == _text.ToLower())
                        .ToList();

                DataGridViewRow matchingRow = resultRows
                    .FirstOrDefault(row => row.Index > dgBody.SelectedRows.TopmostRow().Index);

                return matchingRow == null ? null : matchingRow.Index as object;

                #endregion OpCode
            }

            if (_searchType == SearchType.String)
            {
                #region String

                if (!(_searchNode.Tag is MethodDef)) return null;

                DataGridView dgBody = MainForm.DgBody;

                List<DataGridViewRow> resultRows =
                    dgBody.Rows.Cast<DataGridViewRow>()
                        .Where(
                            r =>
                                r.Cells["OpCode"].Value.ToString().Trim() == "ldstr" &&
                                r.Cells["Operand"].Value.ToString().ToLower().Contains(_text))
                        .ToList();

                DataGridViewRow matchingRow = resultRows
                    .FirstOrDefault(row => row.Index > dgBody.SelectedRows.TopmostRow().Index);

                return matchingRow == null ? null : matchingRow.Index as object;

                #endregion String
            }

            if (_searchType == SearchType.Operand)
            {
                #region Operand

                if (!(_searchNode.Tag is MethodDef)) return null;

                List<DataGridViewRow> resultRows =
                    MainForm.DgBody.Rows.Cast<DataGridViewRow>()
                        .Where(r => r.Cells["Operand"].Value.ToString().ToLower().Contains(_text)).ToList();

                DataGridViewRow matchingRow = resultRows
                    .FirstOrDefault(row => row.Index > MainForm.DgBody.SelectedRows.TopmostRow().Index);

                return matchingRow == null ? null : matchingRow.Index as object;

                #endregion Operand
            }

            return null;
        }
    }
}