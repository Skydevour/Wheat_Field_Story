using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace CommonFramework.Editor.GuiBehaviour
{
    public abstract class TextButtonGuiRenderBase : ButtonGuiRenderBase
    {
        public TextButtonGuiRenderBase(string text, string toolTip, Action onButtonPressedCallback) : base(onButtonPressedCallback)
        {
            guiContent = new GUIContent(text, toolTip);
        }
    }
}
