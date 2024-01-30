using System;
using System.Collections.Generic;

namespace CommonFramework.Editor.UndoRedo
{
    public class UndoRedoBatch : IUndoRedoAction
    {
        private List<Action> undoRecords = new List<Action>();
        private List<Action> redoRecords = new List<Action>();

        public UndoRedoBatch() { }

        public UndoRedoBatch AddUndo(Action undo)
        {
            if (undo != null)
                undoRecords.Add(undo);

            return this;
        }

        public UndoRedoBatch AddRedo(Action redo)
        {
            if (redo != null)
                redoRecords.Add(redo);

            return this;
        }

        public void Undo()
        {
            foreach (var item in undoRecords) { item.Invoke(); }
        }
        public void Redo()
        {
            foreach (var item in redoRecords) { item.Invoke(); }
        }

        public void Clear()
        {
            undoRecords.Clear();
            redoRecords.Clear();
        }
    }
}


