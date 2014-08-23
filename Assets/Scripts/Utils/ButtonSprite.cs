using UnityEngine;
using System;

/*
 *  This class is a sprite that can be used to recive touches, has events for when the user touches it, and is pretty lame :P
 *  You can set it to be activate as long as the user has te finger on the button or only the 1st touch.
 */ 

public class ButtonSprite : FSprite
{
    public delegate void ButtonSpriteSignalDelegate();
    public event ButtonSpriteSignalDelegate SignalPress;
    public event ButtonSpriteSignalDelegate SignalRelease;
    protected FSprite _sprite;
    protected Rect _hitRect;
    public float expansionAmount = 10;
    public bool continuousTouch;

    public ButtonSprite(string elementName, bool continuos = true):base(elementName)
    {
        _sprite = new FSprite(elementName);
        _sprite.anchorX = _anchorX;
        _sprite.anchorY = _anchorY;
        _hitRect = _sprite.textureRect;
        continuousTouch = continuos;
    }

    public virtual void HandleTouch(FTouch touch)
    {
        Vector2 touchPos = this.GlobalToLocal(touch.position);

        Rect expandedRect = _hitRect.CloneWithExpansion(expansionAmount);

        if (expandedRect.Contains(touchPos))
        {
            if (!continuousTouch) {
                if (touch.phase != TouchPhase.Began) {
                    return;
                }
            }
            if (SignalPress != null) SignalPress();
            alpha = 1f;
        }
    }

    public virtual void HandleRemoved() 
    {
        alpha = 0.5f;
        if (SignalRelease != null) SignalRelease();
    }
}