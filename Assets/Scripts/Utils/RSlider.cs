using UnityEngine;
using System;

/*
 *  A simple Slider for doing stuff
 *  Set a background image (the bar) a foreground image (the indicator) and a default initial position.
 *  The slider can go from 0 to 1
 */

public class RSlider : FSprite, FSingleTouchableInterface
{
    public delegate void ButtonSpriteSignalDelegate();
    public event ButtonSpriteSignalDelegate SignalPress;
    public event ButtonSpriteSignalDelegate SignalRelease;
    protected FSprite _sprite;
    protected Rect _hitRect;
    public float expansionAmount = 10;
    FSprite _movableIndicator;
    bool touchInited;
    float indicatorValue;
    string indicatorSprite;

    public float IndicatorValue { get { return indicatorValue; } }

    public RSlider(string elementName, string indicator, float initial = 0.5f)
        : base(elementName)
    {
        _hitRect = base.textureRect;
        expansionAmount = 40;
        indicatorValue = initial;
        _movableIndicator = new FSprite(indicator);
        EnableSingleTouch();
    }

    public void Init()
    {
        base.container.AddChild(_movableIndicator);
        _movableIndicator.SetPosition(GetPosition());
        _movableIndicator.alpha = base.alpha;
        _movableIndicator.x = Mathf.Clamp((base.x - _hitRect.width / 2) + _hitRect.width * IndicatorValue, (base.x - _hitRect.width / 2) + _movableIndicator.width / 2, (base.x + _hitRect.width / 2) - _movableIndicator.width / 2);
    }

    virtual public bool HandleSingleTouchBegan(FTouch touch)
    {
        Vector2 touchPos = base.GetLocalTouchPosition(touch);

        if (_hitRect.Contains(touchPos))
        {
            indicatorValue = Mathf.Abs((base.x - _hitRect.width / 2) - _movableIndicator.x) / _hitRect.width;
            _movableIndicator.x = Mathf.Clamp(LocalToGlobal(touchPos).x, (base.x - _hitRect.width / 2) + _movableIndicator.width / 2, (base.x + _hitRect.width / 2) - _movableIndicator.width / 2);
            return true;
        }

        return false;
    }

    virtual public void HandleSingleTouchMoved(FTouch touch)
    {
        Vector2 touchPos = base.GetLocalTouchPosition(touch);

        //expand the hitrect so that it has more error room around the edges
        //this is what Apple does on iOS and it makes for better usability
        Rect expandedRect = _hitRect.CloneWithExpansion(expansionAmount);

        if (expandedRect.Contains(touchPos))
        {
            indicatorValue = Mathf.Abs((base.x - _hitRect.width / 2) - _movableIndicator.x) / _hitRect.width;
            _movableIndicator.x = Mathf.Clamp(LocalToGlobal(touchPos).x, (base.x - _hitRect.width / 2) + _movableIndicator.width / 2, (base.x + _hitRect.width / 2) - _movableIndicator.width / 2);
            indicatorValue = indicatorValue >= 0.92 ? indicatorValue + 0.1f : indicatorValue;
            indicatorValue = Mathf.Clamp(indicatorValue, 0, 1);
        }
        else
        {
        }
    }

    virtual public void HandleSingleTouchEnded(FTouch touch)
    {

        Vector2 touchPos = base.GetLocalTouchPosition(touch);

        //expand the hitrect so that it has more error room around the edges
        //this is what Apple does on iOS and it makes for better usability
        Rect expandedRect = _hitRect.CloneWithExpansion(expansionAmount);

        if (expandedRect.Contains(touchPos))
        {
            if (SignalRelease != null) SignalRelease();
            indicatorValue = Mathf.Abs((base.x - _hitRect.width / 2) - _movableIndicator.x) / _hitRect.width;
            _movableIndicator.x = Mathf.Clamp(LocalToGlobal(touchPos).x, (base.x - _hitRect.width / 2) + _movableIndicator.width / 2, (base.x + _hitRect.width / 2) - _movableIndicator.width / 2);
            indicatorValue = indicatorValue >= 0.92 ? indicatorValue + 0.1f : indicatorValue;
            indicatorValue = Mathf.Clamp(indicatorValue, 0, 1);
        }
        else
        {
        }
    }

    virtual public void HandleSingleTouchCanceled(FTouch touch)
    {
        _movableIndicator.x = Mathf.Clamp((base.x - _hitRect.width / 2) + _hitRect.width * IndicatorValue, (base.x - _hitRect.width / 2) + _movableIndicator.width / 2, (base.x + _hitRect.width / 2) - _movableIndicator.width / 2);
    }



}