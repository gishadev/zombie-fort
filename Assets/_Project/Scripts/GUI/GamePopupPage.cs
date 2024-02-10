using gishadev.fort.Core;
using gishadev.tools.UI;

namespace gishadev.fort.GUI
{
    public class GamePopupPage : Page
    {
        public void OnRetryButtonClicked() => GameManager.RestartGame();
    }
}