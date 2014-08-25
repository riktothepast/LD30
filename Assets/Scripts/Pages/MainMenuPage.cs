using UnityEngine;
using System.Collections;

public class MainMenuPage : Page
{

	FSprite gameLogo;
    FButton Play;
    FButton Options;
    FLabel title, message;

    public MainMenuPage()
    {
		gameLogo = new FSprite("Futile_White");
		Play = new FButton("Futile_White", "Futile_White");
        title = new FLabel("font", "");
        message = new FLabel("font", "press Enter to begin adventure");
        
        Play.SignalRelease += PlayRelease;

        ListenForUpdate(Update);
    }

    // Use this for initialization
    override public void Start()
    {
		gameLogo.SetPosition(Futile.screen.halfWidth, Futile.screen.height * 6 / 9);
		gameLogo.scaleX = 10f;
		gameLogo.scaleY = 5f;
        // Labels
        message.SetPosition(Futile.screen.width / 2, Futile.screen.height * 3 / 9);
        AddChild(message);
	}

    private void PlayRelease(FButton button)
    {
        Game.instance.GoToPage(PageType.GamePage);
    }



    public void Update() {

        if (Input.GetKeyDown(KeyCode.Return)) {
            Game.instance.GoToPage(PageType.GamePage);
        }

        message.alpha = Mathf.PingPong(Time.time, 1f);
    }
}
