using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dnEditor.Forms;
using dnEditor.Handlers;

namespace dnEditor.Misc
{
    public class NavigationHistory
    {
        public bool HasPast = false;
        public bool HasFuture = false;

        private readonly TreeViewHandler _treeViewHandler;
        private readonly MainForm _mainForm;
        private readonly List<object> _pastHistory = new List<object>();
        private readonly List<object> _futureHistory = new List<object>();
        private bool _clearFuture = true;

        public NavigationHistory(TreeViewHandler treeViewHandler)
        {
            if (!(treeViewHandler.CurrentForm is MainForm))
                throw new Exception("TreeViewHandler form is not MainForm!");

            _treeViewHandler = treeViewHandler;
            _mainForm = (MainForm) _treeViewHandler.CurrentForm;
        }

        public void AddPastHistory(object member)
        {
            //TODO: Fix bug if cycling through two same nodes

            if (_pastHistory.Count < 2 || _pastHistory[_pastHistory.Count - 2] != member)
                _pastHistory.Add(member);  

            if (_clearFuture)
                _futureHistory.Clear();
            else
                _clearFuture = true;

            UpdateState();
        }

        public void GoBack()
        {
            var pastNode = _pastHistory[_pastHistory.Count - 2];
            var currentNode = _pastHistory[_pastHistory.Count - 1];

            _clearFuture = false;
            _treeViewHandler.BrowseAndExpandMember(pastNode);

            _futureHistory.Add(currentNode);
            _pastHistory.Remove(currentNode);

            UpdateState();
        }

        public void GoForward()
        {
            _clearFuture = false;
            _treeViewHandler.BrowseAndExpandMember(_futureHistory.Last());
            _futureHistory.Remove(_futureHistory.Last());

            UpdateState();
        }

        public void Clear()
        {
            _pastHistory.Clear();
            _futureHistory.Clear();
            _clearFuture = true;

            UpdateState();
        }

        private void UpdateState()
        {
            HasPast = _pastHistory.Count > 1;
            HasFuture = _futureHistory.Count > 0;
            _mainForm.HandleToolStripItemsState();
        }
    }
}
