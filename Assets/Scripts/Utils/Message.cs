using UnityEngine;
using System.Collections;

/*
    This will show a message for each game phase, like "Your turn" and then it will fade out.
 */

public class Message : FLabel{
    float ttl;
    public Message(string font, string text, Vector2 position, float scaling = 1)
        : base(font, text)
    {
		ListenForUpdate(Update);
		base.scale = scaling;
        SetPosition(position);
        ttl = 1f;
	}

	public void Update(){
		SetPosition(new Vector2(GetPosition().x,GetPosition().y+Time.deltaTime*100f));
        ttl -= 0.05f;
        alpha -= 0.01f;
        if (ttl <= 0)
        {
			Destroy();
		}
	}

	public void Destroy (){
        RemoveListenForUpdate();
		RemoveFromContainer ();
	}
}
