using UnityEngine;

public class MenuSystemReference : MonoBehaviour
{
    private MenuSystem menuSystem;
    public MenuSystem MenuSystem => menuSystem;

    public void SetMenuSystemReference(MenuSystem menuSystemRef)
    {
        menuSystem = menuSystemRef;
    }

    public void MenuSystemBack()
    {
        menuSystem.Back();
    }

    public void MenuSystemOpenMenu(string menuId)
    {
        menuSystem.OpenLocalMenu(menuId);
    }

}
