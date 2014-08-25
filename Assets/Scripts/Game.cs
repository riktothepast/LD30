using UnityEngine;
using System.Collections.Generic;

public enum PageType
{
		None,
		MenuPage,
		LevelSelectPage,
		OptionsPage,
		GamePage,
        FinalPage
}

public class Game : MonoBehaviour
{

		public static Game instance;
		public FFont myfont;
		private PageType _currentPageType = PageType.None;
		private Page _currentPage = null;
		private FStage _stage;


		// Use this for initialization
		void Start ()
		{
				Application.targetFrameRate = 30;
				QualitySettings.vSyncCount = 0;
				RXDebug.Log ("Starting the game");
				instance = this;
				FSoundManager.Init ();

                FutileParams fparams = new FutileParams(true, true, false, false);
                fparams.targetFrameRate = 30;
				fparams.AddResolutionLevel (480, 1f, 1.0f, ""); //base res
                
				fparams.origin = new Vector2 (0.0f, 0.0f);
		
				Futile.instance.Init (fparams);
				GameConfig.LoadPreviousConfig();

                Futile.atlasManager.LoadAtlas("Atlases/SpriteSheet");
                Futile.atlasManager.LoadFont("font", "font", "Atlases/font", 0, 0);
				_stage = Futile.stage;
		
				GoToPage (PageType.MenuPage);

		}
	
		public void GoToPage (PageType pageType)
		{
				if (_currentPageType == pageType)
						return;
		
				Page pageToCreate = null;

                if (pageType == PageType.MenuPage)
                {
                    pageToCreate = new MainMenuPage();
                }
		
				if (pageType == PageType.GamePage) 
				{
						pageToCreate = new GamePage ();
				}

                if (pageType == PageType.FinalPage)
				{
						pageToCreate = new FinalPage ();
				}

				if (pageToCreate != null) {
						_currentPageType = pageType;	
			
						if (_currentPage != null) {
                            _stage.RemoveAllChildren();
						}
			
						_currentPage = pageToCreate;
						_stage.AddChild (_currentPage);
						_currentPage.Start ();
				}
		
		}

}