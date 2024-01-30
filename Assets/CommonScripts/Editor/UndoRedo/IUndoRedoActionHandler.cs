using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonFramework.Editor.UndoRedo
{
    public interface IUndoRedoActionHandler
    {
        public void ClearAllUndoRedo();
        public void RegisterUndoRedoAction(IUndoRedoAction action);
    }
}
