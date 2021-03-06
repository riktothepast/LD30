﻿using UnityEngine;
using System.Collections.Generic;

public class GamePage : Page
{
    TileMap tileMap;
    bool started;
    FContainer touchControlsContainer;
    public static FContainer HeadsUpDisplay;
    public Vector2 camera = Vector2.zero;
    Player player;

    public Player getPlayer()
    {
        return player;
    }

    public GamePage()
    {
        HeadsUpDisplay = new FContainer();
        Debug.Log("In game page");
        FSprite bg = new FSprite("Futile_White");
        bg.color = Color.black;
        bg.width = Futile.screen.width;
        bg.height = Futile.screen.height;
        AddChild(bg);
        bg.SetPosition(new Vector2(Futile.screen.width / 2, Futile.screen.height/2));
    }

    override public void Start()
    {
        tileMap = new TileMap(this);
        tileMap.LoadTileMap("Text/MainArea");
        tileMap.LevelType = 5;
        player = new Player();
        tileMap.setPlayer(player);
        tileMap.getPlayer().Position = tileMap.getLevelData().getPlayerSpawns()[0];
        Futile.stage.AddChild(tileMap);
        Futile.stage.AddChild(HeadsUpDisplay);
        
        ListenForUpdate(Update);
    }

    public void  ChangeLevel(string value){
        Futile.stage.RemoveChild(tileMap);
        Futile.stage.RemoveChild(HeadsUpDisplay);
        this.RemoveAllChildren();
        Player p = tileMap.getPlayer();
        tileMap = new TileMap(this);
        tileMap.LoadTileMap("Text/"+value);
        tileMap.setPlayer(p);
        tileMap.getPlayer().Position = tileMap.getLevelData().getPlayerSpawns()[0];

        Futile.stage.AddChild(tileMap);
        Futile.stage.AddChild(HeadsUpDisplay);
        p = null;
    }

    Vector2 lastPlayerPos;
    // Update is called once per frame
    public void Update()
    {

        camera = tileMap.getPlayer().GetPosition();
        if(tileMap.getPlayer().GetPosition() == lastPlayerPos){
            camera.y += Input.GetAxis("Vertical") * 100f;
        }
        
        FollowVector(camera);


        tileMap.Update();
        lastPlayerPos = tileMap.getPlayer().GetPosition();
    }


    // move the map based on a player or a centric point around various players
    public void FollowVector(Vector2 position, float speed = 5f)
    {
        float newXPosition = tileMap.x;
        float newYPosition = tileMap.y;

        newXPosition = (Futile.screen.halfWidth - position.x);
        newYPosition = (Futile.screen.halfHeight - position.y);

        // limit screen movement
        if (newXPosition > TileMap.tileSize/2)
            newXPosition = TileMap.tileSize/2;
        if (newXPosition < -tileMap.getSize().x + Futile.screen.halfWidth * 2 + TileMap.tileSize / 2)
            newXPosition = -tileMap.getSize().x + Futile.screen.halfWidth * 2 + TileMap.tileSize / 2;

        if (newYPosition < Futile.screen.halfHeight * 2.0f - TileMap.tileSize / 2)
            newYPosition = Futile.screen.halfHeight * 2.0f - TileMap.tileSize / 2;
        if (newYPosition > tileMap.getSize().y - TileMap.tileSize / 2)
            newYPosition = tileMap.getSize().y - TileMap.tileSize / 2;

        // center on screen for small maps
        if (Futile.screen.halfWidth * 2.0f >= tileMap.getSize().x)
            newXPosition = ((Futile.screen.halfWidth * 2.0f - tileMap.getSize().x) / 2.0f);
        if (Futile.screen.halfHeight * 2.0f >= tileMap.getSize().y)
            newYPosition = Futile.screen.halfHeight * 2.0f - ((Futile.screen.halfHeight * 2.0f - tileMap.getSize().y) / 2.0f);

        // move the map
        tileMap.SetPosition(Vector2.Lerp(tileMap.GetPosition(), new Vector2((int)newXPosition, (int)newYPosition), speed * Time.unscaledDeltaTime));
    }

    public void ScreenShake(float time, float amount) {
        ShakeUtil.Go(tileMap, time, amount);
    }

    public FContainer GetHUD() {
        return HeadsUpDisplay;
    }

    
}