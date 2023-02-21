using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
public class AnyButton : MonoBehaviour
{
    public static void register(CommandCode code, System.Action action)
    {
        onCommand += (x =>
        {
            if (x == code)
                action();
        });
    }

    public static AnyButton sender { get; private set; }

    static event System.Action<CommandCode> onCommand;
    public CommandCode command;
    public bool noSound;
    public bool closeCurrentPopup;
    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
        t = transform.DOPunchScale(Vector3.one * .1f, .2f).SetAutoKill(false);
        t.Pause();
    }
    Tween t;
    void OnClick()
    {
        print("clicking");
        t.Rewind();
        t.Play();
        if (!noSound)
            Audios.o.play("click");
        sender = this;
        if (command != 0)
            onCommand?.Invoke(command);
        if (closeCurrentPopup)
            onCommand?.Invoke(CommandCode.closeCurrentPopup);
    }

    private void Reset()
    {
        // var s = this.getOrAdd<Shadow>();
        // s.effectDistance = new Vector2(2, -2);
    }
}


public enum CommandCode
{
    NONE,
    closeCurrentPopup,
    playEndless,
    playLevel,
    openMultiplayerMatch,
    openCoins,
    openProfile,
    openLevels,
    toggleAudio,
    goHome,
    openShop,
    playWithBot,
    cancelMatch,
    playAgain,
    quitGame,
    retryGame,
    stopGame,
    playNextLevel,
}