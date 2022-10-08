using UnityEngine;

public class LoadinScreen : ScreenElement
{
    public override void Show()
    {
        if (!GameController.IsReloaded)
        {
            base.Show();
            Invoke(nameof(onFakeLoadComplete), Random.Range(1, 2));
        }
        else
        {
            ScreenController.Instance.Reloaded();
        }
    }

    private void onFakeLoadComplete()
    {
        GameController.ChangeState(GameStates.Main);
    }
}