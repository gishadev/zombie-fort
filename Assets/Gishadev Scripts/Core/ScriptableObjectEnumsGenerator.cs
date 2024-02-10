using System.Collections.Generic;
using System.Linq;
using gishadev.tools.Generative;
using UnityEngine;

namespace gishadev.tools.Core
{
    public abstract class ScriptableObjectEnumsGenerator : ScriptableObject
    {
#if UNITY_EDITOR
        public abstract void OnCollectionChanged();

        protected void InitEnumForCollection(IEnumerable<EnumEntryTarget> collection,
            IEnumerable<string> enumEntriesNames,
            string enumName)
        {
            var list = collection.ToList();

            string[] entries = new string[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                entries[i] = enumEntriesNames.ToArray()[i];
                list[i].SetEnumIndex(i);
            }

            EnumGenerator.GenerateEnumClass(enumName, entries);
        }
#endif
    }
}