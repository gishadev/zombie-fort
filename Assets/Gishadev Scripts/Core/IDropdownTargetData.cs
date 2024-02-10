using System.Collections.Generic;

namespace gishadev.tools.Core
{
    public interface IDropdownTargetData
    {
    }

    public interface IDropdownHolder
    {
        void OnDragNDropped<T, U>(U importKeyObject, IEnumerable<T> targetCollection)
            where T : IDropdownTargetData, new()
            where U : class;
    }
}