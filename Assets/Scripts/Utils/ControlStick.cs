using UnityEngine;
using System;

/*
 *  This class is simulating a virtual control stick, you can place the sticke on the left or right side of the screen to make it work
 *  You can set it to be activate as long as the user has te finger on the button or only the 1st touch.
 */

public class ControlStick : ButtonSprite
{
    public event ButtonSpriteSignalDelegate SignalPressA;
    public event ButtonSpriteSignalDelegate SignalPressB;
    FSprite _movableStick;
    bool touchInited;
    bool lefty;
    public ControlStick(string elementName, string elementName2, bool continuos = true, bool lefthanded = false)
        : base(elementName)
    {
        _sprite = new FSprite(elementName);
        _movableStick = new FSprite(elementName2);
        _sprite.anchorX = _anchorX;
        _sprite.anchorY = _anchorY;
        _hitRect = _sprite.textureRect;
        continuousTouch = continuos;
        expansionAmount = 30;
        lefty = lefthanded;
    }

    public void Init()
    {
        _movableStick.scale = base.scale;
        base.container.AddChild(_movableStick);
        _movableStick.SetPosition(GetPosition());
        _movableStick.alpha = base.alpha;
    }

    public override void HandleTouch(FTouch touch)
    {
        Vector2 touchPos = this.GlobalToLocal(touch.position);

        // set the virtual stick in its initial position
        if (lefty)
        {
            if ((touch.position.x < Futile.screen.halfWidth))
            {
                return;
            }
        }
        else
        {
            if ((touch.position.x > Futile.screen.halfWidth))
            {
                return;
            }
        }

        if (touch.phase == TouchPhase.Began)
        {
            base.SetPosition(LocalToGlobal(touchPos));

        }

        _movableStick.x = base.GetPosition().x;
        _movableStick.y = base.GetPosition().y;

        _movableStick.alpha = 0.5f;
        base.alpha = 0.5f;
        Rect expandedRect = _hitRect.CloneWithExpansion(expansionAmount);

        if (expandedRect.Contains(touchPos))
        {

            if (touch.phase != TouchPhase.Began)
            {
                touchInited = true;
            }
            if (touchPos.x > expandedRect.center.x)
            {
                //hit right side of slider?
                if (SignalPressA != null) SignalPressA();

                _movableStick.x = Mathf.Clamp(LocalToGlobal(touchPos).x, (base.x - base.element.sourceRect.width / 6), (base.x + base.element.sourceRect.width / 6));
                _movableStick.y = Mathf.Clamp(LocalToGlobal(touchPos).y, (base.y - base.element.sourceRect.height / 6), (base.y + base.element.sourceRect.height / 6));
            }
            if (touchPos.x < expandedRect.center.x)
            {
                //hit left side of slider?
                if (SignalPressB != null) SignalPressB();
                _movableStick.x = Mathf.Clamp(LocalToGlobal(touchPos).x, (base.x - base.element.sourceRect.width / 6), (base.x + base.element.sourceRect.width / 6));
                _movableStick.y = Mathf.Clamp(LocalToGlobal(touchPos).y, (base.y - base.element.sourceRect.height / 6), (base.y + base.element.sourceRect.height / 6));
            }
        }
        else
        {
            if (!touchInited)
                return;

            if (touchPos.x > expandedRect.center.x)
            {
                //hit right side of slider?
                if (SignalPressA != null) SignalPressA();
                _movableStick.x = Mathf.Clamp(LocalToGlobal(touchPos).x, (base.x - base.element.sourceRect.width / 6), (base.x + base.element.sourceRect.width / 6));
                _movableStick.y = Mathf.Clamp(LocalToGlobal(touchPos).y, (base.y - base.element.sourceRect.height / 6), (base.y + base.element.sourceRect.height / 6));
            }
            if (touchPos.x < expandedRect.center.x)
            {
                //hit left side of slider?
                if (SignalPressB != null) SignalPressB();
                _movableStick.x = Mathf.Clamp(LocalToGlobal(touchPos).x, (base.x - base.element.sourceRect.width / 6), (base.x + base.element.sourceRect.width / 6));
                _movableStick.y = Mathf.Clamp(LocalToGlobal(touchPos).y, (base.y - base.element.sourceRect.height / 6), (base.y + base.element.sourceRect.height / 6));
            }
        }

        if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
        {
            _movableStick.x = base.GetPosition().x;
            _movableStick.y = base.GetPosition().y;
            touchInited = false;
        }

    }

    public override void HandleRemoved()
    {
        base.HandleRemoved();
        _movableStick.alpha = 0;
        base.alpha = 0;
    }

}