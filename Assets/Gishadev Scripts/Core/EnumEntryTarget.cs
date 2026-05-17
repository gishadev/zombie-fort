namespace gishadev.tools.Core
{
    public abstract class EnumEntryTarget
    {
        public int EnumIndex { get; private set; }

        public void SetEnumIndex(int enumIndex)
        {
            EnumIndex = enumIndex;
        }
    }
}