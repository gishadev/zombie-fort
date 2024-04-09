using gishadev.tools.Events;
using gishadev.tools.UI;
using UnityEngine;

namespace gishadev.fort.GUI
{
    public class WinPopupPage : Page
    {
        [SerializeField] private DefaultEventChannelSO winChannelEvent;

        public void OnYesClicked()
        {
            winChannelEvent.ChangeValue(new StringWrapper());
        }
    }
}