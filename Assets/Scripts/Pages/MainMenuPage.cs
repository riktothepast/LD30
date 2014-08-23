using UnityEngine;
using System.Collections;

public class MainMenuPage : Page
{

	FSprite gameLogo;
    FButton Play;
    FButton Options;


    public MainMenuPage()
    {
		gameLogo = new FSprite("Futile_White");
		Play = new FButton("Futile_White", "Futile_White");
        Play.SignalRelease += PlayRelease;
    }

    // Use this for initialization
    override public void Start()
    {
		gameLogo.SetPosition(Futile.screen.halfWidth, Futile.screen.height * 6 / 9);
		gameLogo.scaleX = 10f;
		gameLogo.scaleY = 5f;
        // Labels
		Play.SetPosition(Futile.screen.width/2  , Futile.screen.height * 3 / 9);
		AddChild(gameLogo);
		AddChild(Play);
	
	}

    private void PlayRelease(FButton button)
    {
        Game.instance.GoToPage(PageType.GamePage);
    }

    private void OptionsRelease(FButton button)
    {
        Game.instance.GoToPage(PageType.OptionsPage);
    }
}
