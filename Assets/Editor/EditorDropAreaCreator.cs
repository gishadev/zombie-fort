using System.Collections.Generic;
using gishadev.tools.Core;
using UnityEditor;
using UnityEngine;

namespace gishadev.tools.editor
{
    /// <summary>
    /// Simple creator of dropdown areas for fast fill.
    /// </summary>
    /// <typeparam name="T">Data, which will be filled</typeparam>
    /// <typeparam name="U">Input object</typeparam>
    public static class EditorDropAreaCreator<T, U> 
        where T : IDropdownTargetData, new()
        where U : class
    {
        public static void Create(IDropdownHolder dropdownHolder,
            IEnumerable<T> targetCollection)
        {
            Event evt = Event.current;
            Rect dropArea = GUILayoutUtility.GetRect(0.0f, 50.0f, GUILayout.ExpandWidth(true));
            GUI.Box(dropArea, $"DRAG & DROP FOR {targetCollection.GetType().Name}");

            switch (evt.type)
            {
                case EventType.DragUpdated:
                case EventType.DragPerform:
                    if (!dropArea.Contains(evt.mousePosition))
                        return;

                    DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

                    if (evt.type == EventType.DragPerform)
                    {
                        DragAndDrop.AcceptDrag();

                        foreach (Object draggedObject in DragAndDrop.objectReferences)
                        {
                            if (draggedObject is U importKeyObject)
                                dropdownHolder.OnDragNDropped(importKeyObject, targetCollection);
                        }
                    }

                    break;
            }
            
            
        }
    }
}