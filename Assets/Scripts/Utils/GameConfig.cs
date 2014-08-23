using UnityEngine;
using System.Collections;

public class GameConfig {

	public static bool isMoonWalkingActive;

	public static bool isLeftHandedActive;

	public static float screenShakeIntensity;

    public static float virtualControlsSize;

	public static void LoadPreviousConfig()
	{
		if(PlayerPrefs.HasKey("moonwalk"))
		{
			if(PlayerPrefs.GetInt("moonwalk") == 1)
				isMoonWalkingActive = true;
			else
				isMoonWalkingActive = false;
		}else
		{
			isMoonWalkingActive = true;
		}

		if(PlayerPrefs.HasKey("lefthanded"))
		{
			if(PlayerPrefs.GetInt("lefthanded") == 1)
				isLeftHandedActive = true;
			else
				isLeftHandedActive = false;
		}else
		{
			isLeftHandedActive = false;
		}

		if(PlayerPrefs.HasKey("screenshake"))
		{
			screenShakeIntensity = PlayerPrefs.GetFloat("screenshake");
		}else
		{
			screenShakeIntensity = 1f;
		}

        if (PlayerPrefs.HasKey("virtualcontrolsize"))
        {
            virtualControlsSize = PlayerPrefs.GetFloat("virtualcontrolsize");
        }
        else
        {
            virtualControlsSize = 0.5f;
        }
	}

	public static void saveConfig()
	{
		if(isMoonWalkingActive)
			PlayerPrefs.SetInt("moonwalk",1);
		else
			PlayerPrefs.SetInt("moonwlk",0);

		if(isLeftHandedActive)
			PlayerPrefs.SetInt("lefthanded",1);
		else
			PlayerPrefs.SetInt("lefthanded",0);

		PlayerPrefs.SetFloat("screenshake",screenShakeIntensity);

        PlayerPrefs.SetFloat("virtualcontrolsize", virtualControlsSize);

		PlayerPrefs.Save();
	}

}
