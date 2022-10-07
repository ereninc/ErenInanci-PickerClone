using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenController : ControllerBaseModel
{
    public static ScreenController Instance;
    public ScreenElement ActiveScreen;
    [SerializeField] List<ScreenElement> screens;
    [SerializeField] bool autoChangeScreens;

    public override void Initialize()
    {
        base.Initialize();
        if (Instance != null)
        {
            Destroy(Instance);
        }

        Instance = this;

        foreach (var item in screens)
        {
            item.Initialize();
            item.SetActiveGameObject(false);
        }

        ActiveScreen.Show();

        if (autoChangeScreens)
            GameController.Instance.OnStateChange.AddListener(onStateChange);
    }

    private void onStateChange(GameStates currentState)
    {
        switch (currentState)
        {
            case GameStates.Loading:
                ChangeScreen(false, 0);
                break;
            case GameStates.Main:
                ChangeScreen(true, 1);
                break;
            case GameStates.Game:
                ChangeScreen(true, 2);
                break;
            case GameStates.Win:
                ChangeScreen(true, 3);
                break;
            case GameStates.Lose:
                ChangeScreen(true, 4);
                break;
            default:
                break;
        }
    }

    public static void ChangeScreen(bool showAfterHide, int index)
    {
        ScreenElement nextScreen = GetScreen<ScreenElement>(index);

        if (showAfterHide)
        {
            if (Instance.ActiveScreen != null)
            {
                Instance.ActiveScreen.Hide(nextScreen.Show);
                Instance.ActiveScreen = nextScreen;
            }
            else
            {
                Instance.ActiveScreen = nextScreen;
                Instance.ActiveScreen.Show();
            }
        }
        else
        {
            Instance.ActiveScreen.Hide();
            Instance.ActiveScreen = nextScreen;
            Instance.ActiveScreen.Show();
        }

    }

    public static T GetScreen<T>()
    {
        return (T)(object)Instance.screens.Find(x => x.GetType() == typeof(T));
    }

    public static T GetScreen<T>(int index)
    {
        return (T)(object)Instance.screens[index];
    }

    public void Reloaded()
    {
        ActiveScreen = GetScreen<MainScreen>();
        ActiveScreen.Show();
        GameController.ChangeState(GameStates.Main);
    }
}
