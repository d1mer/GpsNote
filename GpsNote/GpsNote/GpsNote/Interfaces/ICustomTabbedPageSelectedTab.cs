namespace GpsNote.Interfaces
{
    public interface ICustomTabbedPageSelectedTab
    {
        int SelectedTab { get; set; }
        void SetSelectedTab(int tabIndex);
    }
}