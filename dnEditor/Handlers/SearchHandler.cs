using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using dnEditor.Forms;
using dnEditor.Misc;
using dnlib.DotNet;
using dnlib.DotNet.Emit;

namespace dnEditor.Handlers
{
    public enum SearchType
    {
        Any,
        MDToken,
        StringGlobal,
        String,
        OpCode,
        Operand
    }

    public class SearchHandler
    {
        public delegate void EventHandler(object result);

        private readonly TreeNode _searchNode;
        private readonly string _searchText;
        private readonly TreeView _searchTreeView;
        private readonly SearchType _searchType;
        private readonly TreeViewHandler _treeViewHandler;
        public TreeNode FoundNode = null;

        public SearchHandler(TreeNode searchNode, string text, SearchType searchType, TreeViewHandler treeViewHandler)
        {
            _searchNode = searchNode;
            _searchTreeView = searchNode.TreeView;

            _searchText = text.ToLower();
            _searchType = searchType;
            _treeViewHandler = treeViewHandler;
        }

        public SearchHandler(TreeView searchTreeView, string text, SearchType searchType, TreeViewHandler treeViewHandler)
        {
            _searchTreeView = searchTreeView;
            _searchText = text.ToLower();
            _searchType = searchType;
            _treeViewHandler = treeViewHandler;
        }

        public event EventHandler SearchFinished;

        public void Search()
        {
            AnalysisHandler.Reset();

            object result = DoSearch();

            if (SearchFinished != null)
                SearchFinished.Invoke(result);

            if (_searchType == SearchType.Any || _searchType == SearchType.MDToken || _searchType == SearchType.StringGlobal)
                AnalysisHandler.SelectTab();
        }

        private object DoSearch()
        {
            if (_searchType == SearchType.Any)
            {
                return new SearchAny(_treeViewHandler, _searchText).Search();
            }

            if (_searchType == SearchType.MDToken)
            {
                return new SearchMDToken(_treeViewHandler, _searchText).Search();
            }

            if (_searchType == SearchType.StringGlobal)
            {
                return new SearchStringGlobal(_treeViewHandler, _searchText).Search();
            }

            if (_searchType == SearchType.String)
            {
                return SearchStringInMethod();
            }

            if (_searchType == SearchType.OpCode)
            {
                return SearchOpCode();
            }

            if (_searchType == SearchType.Operand)
            {
                return SearchOperand();
            }

            return null;
        }

        private object SearchOperand()
        {
            if (!(_searchNode.Tag is MethodDef)) return null;

            List<DataGridViewRow> resultRows = MainForm.DgBody.Rows.Cast<DataGridViewRow>().Where(r => 
                r.Cells["Operand"].Value.ToString().ToLower().Contains(_searchText)).ToList();

            DataGridViewRow matchingRow = resultRows.FirstOrDefault(row => row.Index > MainForm.DgBody.SelectedRows.TopmostRow().Index);

            return matchingRow == null ? null : matchingRow.Index as object;
        }

        private object SearchStringInMethod()
        {
            if (!(_searchNode.Tag is MethodDef)) return null;

            DataGridView dgBody = MainForm.DgBody;

            List<DataGridViewRow> resultRows =
                dgBody.Rows.Cast<DataGridViewRow>().Where(
                    r =>
                        r.Cells["OpCode"].Value.ToString().Trim() == "ldstr" &&
                        r.Cells["Operand"].Value.ToString().ToLower().Contains(_searchText)).ToList();

            DataGridViewRow matchingRow =
                resultRows.FirstOrDefault(row => row.Index > dgBody.SelectedRows.TopmostRow().Index);

            return matchingRow == null ? null : matchingRow.Index as object;
        }

        private object SearchOpCode()
        {
            if (!(_searchNode.Tag is MethodDef)) return null;

            DataGridView dgBody = MainForm.DgBody;

            List<DataGridViewRow> resultRows =
                dgBody.Rows.Cast<DataGridViewRow>().Where(
                    r => r.Cells["OpCode"].Value.ToString().ToLower().Trim() == _searchText.ToLower()).ToList();

            DataGridViewRow matchingRow =
                resultRows.FirstOrDefault(row => row.Index > dgBody.SelectedRows.TopmostRow().Index);

            return matchingRow == null ? null : matchingRow.Index as object;
        }
    }

    public class SearchAny
    {
        private readonly string _searchText;
        private readonly TreeViewHandler _treeViewHandler;

        public SearchAny(TreeViewHandler treeViewHandler, string searchText)
        {
            _treeViewHandler = treeViewHandler;
            _searchText = searchText;
        }

        public List<object> Search()
        {
            var results = new List<object>();

            var currentModule = _treeViewHandler.CurrentModule.Tag as ModuleDefMD;
            if (currentModule == null)
                return results;

            foreach (TypeDef type in currentModule.GetTypes())
                results.AddRange(ProcessChildren(type));

            return results;
        }

        public List<object> ProcessChildren(TypeDef type)
        {
            var results = new List<object>();

            if (CheckMember(type))
                results.Add(type);

            results.AddRange(type.Methods.Where(CheckMember));
            results.AddRange(type.Fields.Where(CheckMember));
            results.AddRange(type.Properties.Where(CheckMember));
            results.AddRange(type.Events.Where(CheckMember));

            return results;
        }

        public bool CheckMember(object member)
        {
            if (member is TypeDef)
            {
                if ((member as TypeDef).Name.ToLower().Contains(_searchText))
                    return true;
            }
            else if (member is MethodDef)
            {
                if ((member as MethodDef).Name.ToLower().Contains(_searchText))
                    return true;
            }
            else if (member is FieldDef)
            {
                if ((member as FieldDef).Name.ToLower().Contains(_searchText))
                    return true;
            }
            else if (member is PropertyDef)
            {
                if ((member as PropertyDef).Name.ToLower().Contains(_searchText))
                    return true;
            }
            else if (member is EventDef)
            {
                if ((member as EventDef).Name.ToLower().Contains(_searchText))
                    return true;
            }
            return false;
        }
    }

    public class SearchMDToken
    {
        private readonly string _searchText;
        private readonly TreeViewHandler _treeViewHandler;

        public SearchMDToken(TreeViewHandler treeViewHandler, string searchText)
        {
            _treeViewHandler = treeViewHandler;
            _searchText = searchText;
        }

        public List<object> Search()
        {
            var results = new List<object>();

            var currentModule = _treeViewHandler.CurrentModule.Tag as ModuleDefMD;
            if (currentModule == null)
                return results;

            foreach (TypeDef type in currentModule.GetTypes())
                results.AddRange(ProcessChildren(type));

            return results;
        }

        public List<object> ProcessChildren(TypeDef type)
        {
            var results = new List<object>();

            if (CheckMember(type))
                results.Add(type);

            results.AddRange(type.Methods.Where(CheckMember));
            results.AddRange(type.Fields.Where(CheckMember));
            results.AddRange(type.Properties.Where(CheckMember));
            results.AddRange(type.Events.Where(CheckMember));

            return results;
        }

        public bool CheckMember(object member)
        {
            if (member is TypeDef)
            {
                if (string.Format("0x{0}", (member as TypeDef).MDToken.FullMetadataTokenString()) == _searchText ||
                    (member as TypeDef).MDToken.ToInt32().ToString() == _searchText)
                    return true;
            }
            else if (member is MethodDef)
            {
                if (string.Format("0x{0}", (member as MethodDef).MDToken.FullMetadataTokenString()) == _searchText ||
                    (member as MethodDef).MDToken.ToInt32().ToString() == _searchText)
                    return true;
            }
            else if (member is FieldDef)
            {
                if (string.Format("0x{0}", (member as FieldDef).MDToken.FullMetadataTokenString()) == _searchText ||
                    (member as FieldDef).MDToken.ToInt32().ToString() == _searchText)
                    return true;
            }
            else if (member is PropertyDef)
            {
                if (string.Format("0x{0}", (member as PropertyDef).MDToken.FullMetadataTokenString()) == _searchText ||
                    (member as PropertyDef).MDToken.ToInt32().ToString() == _searchText)
                    return true;
            }
            else if (member is EventDef)
            {
                if (string.Format("0x{0}", (member as EventDef).MDToken.FullMetadataTokenString()) == _searchText ||
                    (member as EventDef).MDToken.ToInt32().ToString() == _searchText)
                    return true;
            }
            return false;
        }
    }

    public class SearchStringGlobal
    {
        private readonly string _searchText;
        private readonly TreeViewHandler _treeViewHandler;

        public SearchStringGlobal(TreeViewHandler treeViewHandler, string searchText)
        {
            _treeViewHandler = treeViewHandler;
            _searchText = searchText;
        }

        public Dictionary<object, string> Search()
        {
            var results = new Dictionary<object, string>();

            var currentModule = _treeViewHandler.CurrentModule.Tag as ModuleDefMD;
            if (currentModule == null)
                return results;

            foreach (TypeDef type in currentModule.GetTypes())
            {
                Dictionary<object, string> output = ProcessChildren(type);

                foreach (KeyValuePair<object, string> pair in output)
                    results.Add(pair.Key, pair.Value);
            }

            return results;
        }

        public Dictionary<object, string> ProcessChildren(TypeDef type)
        {
            var results = new Dictionary<object, string>();

            foreach (MethodDef method in type.Methods)
            {
                string output = CheckMember(method);

                if (!string.IsNullOrEmpty(output))
                    results.Add(method, output);
            }

            return results;
        }

        public string CheckMember(object member)
        {
            if (!(member is MethodDef))
                return null;

            var method = (MethodDef) member;

            if (!method.HasBody || !method.Body.HasInstructions)
                return null;

            Instruction result =  method.Body.Instructions.FirstOrDefault(i => i.OpCode == OpCodes.Ldstr && 
                i.Operand != null && i.Operand.ToString().ToLower().Contains(_searchText));

            return result == null ? null : result.Operand.ToString();
        }
    }
}