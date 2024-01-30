using System;

namespace CommonFramework.Editor.UndoRedo
{
    public interface IUndoRedoAction
    {
        public void Undo();

        public void Redo();

        public void Clear();
    }
}
