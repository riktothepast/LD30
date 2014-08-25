using UnityEngine;
using System.Collections;

public class FinalPage : Page
{

    FSprite gameLogo;
    FButton Play;
    FButton Options;
    FLabel title, message;

    public FinalPage()
    {
        
        title = new FLabel("font", "Congratulations!, you made it, you are awesome. \n Thanks for playing this crappy game");
        title.scale = 0.9f;
        message = new FLabel("font", "press Enter to play again.");
        message.scale = 0.8f;

        ListenForUpdate(Update);
    }

    // Use this for initialization
    override public void Start()
    {
        title.SetPosition(Futile.screen.halfWidth, Futile.screen.height * 6 / 9);

        // Labels
        message.SetPosition(Futile.screen.width / 2, Futile.screen.height * 3 / 9);
        AddChild(message);
        AddChild(title);
    }

    private void PlayRelease(FButton button)
    {
        Game.instance.GoToPage(PageType.GamePage);
    }

    public void Update()
    {

        if (Input.GetKeyDown(KeyCode.Return))
        {
            Game.instance.GoToPage(PageType.MenuPage);
        }

        message.alpha = Mathf.PingPong(Time.time, 1f);
    }
}
