using UnityEngine;
using System.Collections;

public class OptionsPage : Page {

	FLabel moonwalkLabel;
	FLabel lefthandedLabel;
	FLabel  screenshakeLabel;
    FLabel controlsScale;
	FButton moonwalkButton;
	FButton lefthandedButton;
    FButton SaveAndReturn;
    RSlider shakeSlider;
    RSlider scaleSlider;

	public OptionsPage()
	{
        FSprite bg = new FSprite("Futile_White");
        bg.width = Futile.screen.width;
        bg.height = Futile.screen.height;
        bg.color = Color.gray;
        bg.x = Futile.screen.width / 2;
        bg.y = Futile.screen.height / 2;
        AddChild(bg);
		// Labels
		moonwalkLabel = new FLabel("font","Moon walk");
		lefthandedLabel = new FLabel("font","Left handed controls");
		screenshakeLabel = new FLabel("font","Screenshake intensity");
        controlsScale = new FLabel("font", "Controls scale");

		moonwalkLabel.alignment = FLabelAlignment.Left;
		lefthandedLabel.alignment = FLabelAlignment.Left;
		screenshakeLabel.alignment = FLabelAlignment.Left;
        controlsScale.alignment = FLabelAlignment.Left;

        SaveAndReturn = new FButton("Futile_White", "Futile_White");
        SaveAndReturn.SignalRelease += SaveAndReturnSignal;

		// Buttons and probs the slider for screenshake?
		moonwalkButton = new FButton("radioOn");
        moonwalkButton.expansionAmount = 40f;
		if(GameConfig.isMoonWalkingActive)
            moonwalkButton.SetElements("radioOn", "radioOn", "radioOn");
		else
            moonwalkButton.SetElements("radioOff", "radioOff", "radioOff");

		moonwalkButton.SignalRelease += HandleMoonWalkButton;

        lefthandedButton = new FButton("radioOn");
        lefthandedButton.expansionAmount = 40f;
		if(GameConfig.isLeftHandedActive)
			lefthandedButton.SetElements("radioOn", "radioOn","radioOn");
		else
            lefthandedButton.SetElements("radioOff", "radioOff", "radioOff");

		lefthandedButton.SignalRelease += HandleLeftiesButton;

        shakeSlider = new RSlider("sliderBar", "sliderIndicator", GameConfig.screenShakeIntensity);
        scaleSlider = new RSlider("sliderBar", "sliderIndicator", GameConfig.virtualControlsSize);
	}

	// Use this for initialization
	override public void Start () {
        // Labels
		moonwalkLabel.SetPosition(Futile.screen.halfWidth / 4,  Futile.screen.height * 2/9);
        lefthandedLabel.SetPosition(Futile.screen.halfWidth / 4, Futile.screen.height * 4 / 9);
        screenshakeLabel.SetPosition(Futile.screen.halfWidth / 4, Futile.screen.height * 6 / 9);
        controlsScale.SetPosition(Futile.screen.halfWidth / 4, Futile.screen.height * 8 / 9);
		AddChild(moonwalkLabel);
		AddChild(lefthandedLabel);
		AddChild(screenshakeLabel);
        AddChild(controlsScale);

		// Buttons and slider
        moonwalkButton.SetPosition(Futile.screen.halfWidth / 2 + Futile.screen.halfWidth, Futile.screen.height * 2 / 9);
        lefthandedButton.SetPosition(Futile.screen.halfWidth / 2 + Futile.screen.halfWidth, Futile.screen.height * 4 / 9);
        SaveAndReturn.SetPosition(Futile.screen.halfWidth, Futile.screen.height * 1 / 9);
        SaveAndReturn.AddLabel("font", "Save & Return", Color.green);
		AddChild(moonwalkButton);
		AddChild(lefthandedButton);
        AddChild(SaveAndReturn);

        // sliders
        shakeSlider.SetPosition(Futile.screen.halfWidth / 2 + Futile.screen.halfWidth, Futile.screen.height * 6 / 9);
        shakeSlider.SignalRelease += SetShakeValue;
        AddChild(shakeSlider);
        shakeSlider.Init();

        scaleSlider.SetPosition(Futile.screen.halfWidth / 2 + Futile.screen.halfWidth, Futile.screen.height * 8 / 9);
        scaleSlider.SignalRelease += SetScaleValue;
        AddChild(scaleSlider);
        scaleSlider.Init();
	}

	private void HandleMoonWalkButton(FButton button)
	{
		if(GameConfig.isMoonWalkingActive)
		{
			GameConfig.isMoonWalkingActive = false;
            moonwalkButton.SetElements("radioOff", "radioOff", "radioOff");
		}else
		{
			GameConfig.isMoonWalkingActive = true;
            moonwalkButton.SetElements("radioOn", "radioOn", "radioOn");
		}
	}

	private void HandleLeftiesButton(FButton button)
	{
		if(GameConfig.isLeftHandedActive)
		{
			GameConfig.isLeftHandedActive = false;
            lefthandedButton.SetElements("radioOff", "radioOff", "radioOff");
		}else
		{
			GameConfig.isLeftHandedActive = true;
            lefthandedButton.SetElements("radioOn", "radioOn", "radioOn");
		}
	}

    private void SaveAndReturnSignal(FButton button)
    {
        GameConfig.saveConfig();
        Game.instance.GoToPage(PageType.MenuPage);
    }

    public void SetShakeValue() {
       GameConfig.screenShakeIntensity = shakeSlider.IndicatorValue;
    }
    public void SetScaleValue()
    {
        GameConfig.virtualControlsSize = scaleSlider.IndicatorValue;
    }
}
