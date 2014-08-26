//
// TileMap.cs
//
// Author:
//       Rik <>
//
// Copyright (c) 2014 Rik
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class TileMap : FContainer
{
    FContainer projectileContainer;
    FContainer particleContainer;
    FContainer entityContainer;
    FParticleSystem particles;
    FContainer enemyContainer;
    FContainer frontContainer;
    List<Bullet> playerBullets;
    List<Portal> portals;
    List<Hazard> hazards;
    List<Item> items;
    List<Entity> enemies;
    FLabel stats;
    //List<Enemies> enemies;
    public int LevelType { get; set; }
    int[,] Tiles = new int[32, 18];
    int maxEnemyCount = 50;
    //List<Platform> tileSprites = new List<Platform>();
    LevelData levelData;
    FLabel messageLabel;
    public FLabel MessageLabel { get { return messageLabel; } set { messageLabel = value; } }
    public int Columns { get; set; }
    public int Rows { get; set; }

    Player player;
    public static TileMap CurrentMap { get; private set; }

    public Player getPlayer() {
        return player;
    }

    public void setPlayer(Player p)
    {
        player = p;
        entityContainer.AddChild(player);
    }

    public static float tileSize { get; set; }

    public GamePage CurrentPage { get; private set; }

    public LevelData getLevelData() { return levelData; }

    public FParticleSystem GetParticleSystem { get { return particles; } }



    public int MaxEnemyCount { get { return maxEnemyCount; } }

    int lengthI, lengthJ;

    public TileMap(GamePage gp)
    {
        CurrentPage = gp;
		Rows = 32;
		Columns = 18;
    }

    public void LoadTileMap(String mapText)
    {
        Array.Clear(Tiles, 0, Tiles.Length);
        levelData = new LevelData();
        levelData.LevelDataXML(mapText);
        Tiles = new int[(int)levelData.getTileMapSize().x, (int)levelData.getTileMapSize().y];
        Debug.Log(levelData.getTileMapSize().x + " " + levelData.getTileMapSize().y);
        // fill TileMap with zeroes.
        loadTiles();
        TileMap.CurrentMap = this;
        // size of the map
        lengthI = Tiles.GetLength(1);
        lengthJ = Tiles.GetLength(0);

        portals = getLevelData().getPortalSpawns();

        for (int x = portals.Count - 1; x >= 0; x--)
        {
            portals[x].SetPosition(portals[x].getPosition());
            AddChild(portals[x]);
        }

        hazards = getLevelData().getHazardSpawns();

        for (int x = hazards.Count - 1; x >= 0; x--)
        {
            FSprite haz = new FSprite("hazard");
            haz.SetPosition(hazards[x].getPosition());
            AddChild(haz);
        }

        for (int x = getLevelData().getEnemySpawns().Count - 1; x >= 0; x--)
        {

        }
        items = new List<Item>();
        if (getLevelData().getItemSpawns() !=null)
        for (int x = getLevelData().getItemSpawns().Count - 1; x >= 0; x--)
        {
            Item it = new Item(getLevelData().getItemSpawns()[x]);
            it.SetPosition(it.getPosition());
            items.Add(it);
            AddChild(it);
        }
        enemies = new List<Entity>();
        if (getLevelData().getEnemySpawns() != null)
            for (int x = getLevelData().getEnemySpawns().Count - 1; x >= 0; x--)
            {
                EnemyPortal it = new EnemyPortal(getLevelData().getEnemySpawns()[x]);
                it.SetPosition(it.Position);
                enemies.Add(it);
                AddChild(it);
            }
    }

    void loadTiles()
    {
        foreach (PlatformData platform in levelData.getPlatformData())
        {
            FSprite plat = new FSprite(platform.image);
            plat.height = 16;
            plat.width = 16;
            tileSize = plat.width;
            Tiles[(int)platform.x, (int)platform.y] = 1;
            // make them overlap a little to avoid those ungly lines in the rendering.
            plat.SetPosition(new Vector2((platform.x * plat.width), (-platform.y * plat.height)));

            AddChild(plat);
        }
        playerBullets = new List<Bullet>();
        hazards = new List<Hazard>();
        particleContainer = new FContainer();
        AddChild(particleContainer);
        enemyContainer = new FContainer();
        AddChild(enemyContainer);
        particles = new FParticleSystem(50);
        particleContainer.AddChild(particles);
        projectileContainer = new FContainer();
        AddChild(projectileContainer);
        entityContainer = new FContainer();
        AddChild(entityContainer);
        frontContainer = new FContainer();
        AddChild(frontContainer);
        this.alpha = 0;

        TweenConfig tw = new TweenConfig();
        tw.floatProp("alpha", 1);
        Go.to(this, 10f, tw);

        stats = new FLabel("font", "Jump Packs: 0; reload times: 0");
        CurrentPage.GetHUD().AddChild(stats);
        stats.scale = 0.9f;
        stats.SetPosition(new Vector2(Futile.screen.halfWidth, Futile.screen.height * 0.9f));
    }

    // update the tilemap based on whats visible ?. 
    public void Update()
    {
        player.Update();

        //update all the bullets
        for (int x = playerBullets.Count - 1; x >= 0; x--) 
        {
            playerBullets[x].Update();
            if (playerBullets[x].Remove)
            {
                playerBullets.Remove(playerBullets[x]);
            }
        }

        for (int x = portals.Count - 1; x >= 0; x--)
        {
            if (Rectangle.AABBCheck(portals[x].getAABBBoundingBox(), getPlayer().getAABBBoundingBox()))
            {
                Debug.Log("will transport to: " + portals[x].getDestination());

                switch (portals[x].getDestination())
                {
                    case 0:
                        CurrentPage.ChangeLevel("MainArea");
                        break;
                    case 1:
                        CurrentPage.ChangeLevel("MainArea");
                        break;
                    case 2:
                        CurrentPage.ChangeLevel("otherLevel");
                        break;
                    case 3:
                        CurrentPage.ChangeLevel("vertical");
                        break;

                    case 5:
                        Game.instance.GoToPage(PageType.FinalPage);
                        break;
                    case 6:
                        Game.instance.GoToPage(PageType.FinalPage);
                        break;
                }

            }
        }




        for (int x = hazards.Count - 1; x >= 0; x--)
        {
            if (Rectangle.AABBCheck(hazards[x].getAABBBoundingBox(), getPlayer().getAABBBoundingBox()))
            {
                player.HealthPoints = 0;
            }
        }
        
        for (int x = enemies.Count - 1; x >= 0; x--)
        {
            if (Rectangle.AABBCheck(enemies[x].getAABBBoundingBox(), getPlayer().getAABBBoundingBox()))
            {
                player.HealthPoints = 0;
            }
            if (enemies[x].Remove)
            {
                enemies.Remove(enemies[x]);
            }
        }


        for (int x = items.Count - 1; x >= 0; x--)
        {
            if (Rectangle.AABBCheck(items[x].getAABBBoundingBox(), getPlayer().getAABBBoundingBox()))
            {
                if (items[x].getDestination() == 0)
                {
                    items[x].RemoveFromContainer();
                    items.Remove(items[x]);
                    AddChild(new Message("font", "empty!", player.Position));

                }else
                //jump
                    if (items[x].getDestination() == 1)
                    {
                        items[x].RemoveFromContainer();
                        items.Remove(items[x]);
                        player.BackPackFuel = player.BackPackFuel + 1;
                        AddChild(new Message("font", "jump upgrade!", player.Position));
                    }
                    else {
                        items[x].RemoveFromContainer();
                        items.Remove(items[x]);
                        player.FiringCadence = player.FiringCadence - 100;
                        AddChild(new Message("font", "blaster upgrade!", player.Position));

                    }
            }
        }

        if (LevelType == 5) {
            ShakeUtil.Go(this, 0.3f, UnityEngine.Random.Range(5, 10));
        }

        stats.text = "Jump Packs: "+ player.BackPackFuel + "; reload times (ms):" + player.FiringCadence;
    }
   

    public Vector2 getSize()
    {
        return new Vector2(Tiles.GetLength(0) * tileSize * 0.99f, Tiles.GetLength(1) * tileSize * 0.99f);
    }

    public Vector2 getSizeIntiles()
    {
        return new Vector2(Tiles.GetLength(0), Tiles.GetLength(1));
    }

    public int getTileType(int x, int y)
    {
        if ((x > 0 && x < Tiles.GetLength(0)) && (y > 0 && y < Tiles.GetLength(1)))
        {
            return Tiles[x, y];
        }
        else
        {
            return -1;
        }

    }
    Rectangle tileRectangle = new Rectangle(0, 0, 0, 0);
    public bool HasRoomForRectangle(Rectangle rectangleToCheck)
    {
        int P_X = Math.Abs((int)((rectangleToCheck.x) / tileSize));
        int P_Y = Math.Abs((int)((rectangleToCheck.y) / tileSize));

        int Min_X = P_X - 1;
        int Max_X = P_X + 2;
        int Min_Y = P_Y - 1;
        int Max_Y = P_Y + 1;

        if (Min_X < 0) Min_X = 0;
        if (Max_X > lengthJ - 1) Max_X = lengthJ - 1;
        if (Min_Y < 0) Min_Y = 0;
        if (Max_Y > lengthI - 1) Max_Y = lengthI - 1;

        for (int j = Min_Y; j <= Max_Y; j++)
        {
            for (int i = Min_X; i <= Max_X; i++)
            {
                tileRectangle.setValues(i * tileSize, -j * tileSize, tileSize, tileSize);
                if (Tiles[i, j] == 1 && Rectangle.AABBCheck(tileRectangle, rectangleToCheck))
                {
                    return false;
                }
            }
        }
        return true;
    }

    MovementWrapper move = new MovementWrapper(Vector2.zero, Vector2.zero, new Rectangle());
    Rectangle newBoundary = new Rectangle();
    public Vector2 WhereCanIGetTo(Vector2 originalPosition, Vector2 destination, Rectangle boundingRectangle)
    {
        move.setValues(originalPosition, destination, boundingRectangle);

        for (int i = 1; i <= move.NumberOfStepsToBreakMovementInto; i++)
        {
            Vector2 positionToTry = originalPosition + move.OneStep * i;
            newBoundary.setValues((int)positionToTry.x, (int)positionToTry.y, boundingRectangle.w, boundingRectangle.h);
            //drawHitBox(newBoundary, Color.black);
            if (HasRoomForRectangle(newBoundary)) { move.FurthestAvailableLocationSoFar = positionToTry; }
            else
            {
                if (move.IsDiagonalMove)
                {
                    move.FurthestAvailableLocationSoFar = CheckPossibleNonDiagonalMovement(move, i);
                }
                break;
            }
        }
        return move.FurthestAvailableLocationSoFar;
    }

    private Vector2 CheckPossibleNonDiagonalMovement(MovementWrapper wrapper, int i)
    {
        if (wrapper.IsDiagonalMove)
        {
            int stepsLeft = wrapper.NumberOfStepsToBreakMovementInto - (i - 1);

            Vector2 remainingHorizontalMovement = wrapper.OneStep.x * Vector2.right * stepsLeft;
            wrapper.FurthestAvailableLocationSoFar =
                WhereCanIGetTo(wrapper.FurthestAvailableLocationSoFar, wrapper.FurthestAvailableLocationSoFar + remainingHorizontalMovement, wrapper.BoundingRectangle);

            Vector2 remainingVerticalMovement = wrapper.OneStep.y * Vector2.up * stepsLeft;
            wrapper.FurthestAvailableLocationSoFar =
                WhereCanIGetTo(wrapper.FurthestAvailableLocationSoFar, wrapper.FurthestAvailableLocationSoFar + remainingVerticalMovement, wrapper.BoundingRectangle);
        }

        return wrapper.FurthestAvailableLocationSoFar;
    }
    /// <summary>World wrapping aka "Pacman" effect 
    /// <para>If the entity is out of bounds, re-set it's position to the first available space in the opposite side of the map.</para>
    /// </summary> 
 
    public Vector2 WordWrap(Rectangle pos) {
        Vector2 toReturn = Vector2.zero;
        //right side of the map
        if (pos.cX > tilesToPixels(lengthJ - 1 ))
        {
            toReturn.x = tilesToPixels(0) + 1;
        }
        //left side of the map
        if (pos.cX < tilesToPixels(0))
        {
            toReturn.x = tilesToPixels(lengthJ - 1) - 1;
        }

        if (pos.cY < -tilesToPixels(lengthI - 1))
        {
            toReturn.y = tilesToPixels(0) + 1;
        }

        if (pos.cY >= -tilesToPixels(0))
        {
            toReturn.y = -tilesToPixels(lengthI - 1) -1;
        }

        return toReturn;
    }
    /// <summary> Returns if the entity is out of map bounds.
    /// <para>Give a position vector, returns a boolean</para>
    /// </summary> 

    public bool IsOutOfBounds(Vector2 position) { 
        if ((position.y < -tilesToPixels(lengthI - 1)))
        {
            return true;
        }

        return false;
    }

    /// <summary> Returns the value in tile size.
    /// <para>given a value in pixel metrics, return its tilemap (logic) position..</para>
    /// </summary> 
    public float pixelsToTiles(float value){
        return (int)(value / tileSize);
    }
        /// <summary> Returns the value in pixel size.
    /// <para>given a value in tile metrics, return its screen (graphic) position..</para>
    /// </summary> 
    public float tilesToPixels(float value){
        return (int)(value * tileSize);

    }
    /// <summary> Returns the value in tile position.
    /// <para>given a vector in pixel size, return its tilemap (logic) position</para>
    /// </summary> 
    /// 
    public Vector2 PositionInTileMap(Vector2 pos) {
        return new Vector2(pixelsToTiles(pos.x), pixelsToTiles(pos.y));
    }

    public FContainer ProjectileContainer() {
        return projectileContainer;
    }

    public FContainer EnemyContainer()
    {
        return enemyContainer;
    }
    public FContainer ParticleContainer()
    {
        return particleContainer;
    }

    public FContainer EntityContainer()
    {
        return entityContainer;
    }

    public FContainer FrontContainer()
    {
        return frontContainer;
    }

    public List<Bullet> getPlayerBulletList()
    {
        return playerBullets;
    }

    public List<Entity> getEnemyList()
    {
        return enemies;
    }

    public void drawHitBox(Rectangle rect, Color color)
    {
        Vector3 tL = new Vector3(0f, 0f, 0f);
        Vector3 tR = new Vector3(0f, 0f, 0f);
        Vector3 bL = new Vector3(0f, 0f, 0f);
        Vector3 bR = new Vector3(0f, 0f, 0f);
        tL.x = rect.x;
        tL.y = rect.y;
        tR.x = rect.x + rect.w;
        tR.y = rect.y;
        bL.x = rect.x;
        bL.y = rect.y + rect.h;
        bR.x = rect.x + rect.w;
        bR.y = rect.y + rect.h;

        Debug.DrawLine(tL, tR, color);
        Debug.DrawLine(tR, bR, color);
        Debug.DrawLine(bR, bL, color);
        Debug.DrawLine(bL, tL, color);
    }

    FParticleDefinition dustParticles;

    public void generateDustParticles(int ceil, Vector2 position, Color initial, Color final, Vector2 sp, float endScale = 0.5f)
    {
        dustParticles = new FParticleDefinition("Shoot_0");
        dustParticles.endScale = endScale;
        dustParticles.startScale = 0.1f;
        dustParticles.startColor = initial;
        dustParticles.endColor = final;
        int part = UnityEngine.Random.Range(1, ceil);
        for (int x = 0; x < part; x++)
        {
            dustParticles.x = position.x;
            dustParticles.y = position.y;
            Vector2 speed = RXRandom.Vector2Normalized() * RXRandom.Range(sp.x, sp.y);
            dustParticles.speedX = speed.x;
            dustParticles.speedY = speed.y;
            dustParticles.lifetime = RXRandom.Range(0.5f, 0.8f);
            TileMap.CurrentMap.GetParticleSystem.AddParticle(dustParticles);
        }
    }

    IEnumerator FadeIn() {

        yield return new WaitForSeconds(5);
        alpha = 1f;
    }

    public void CallGameOver()
    {
        player.RemoveFromContainer();
        TweenConfig tw = new TweenConfig();
        tw.floatProp("alpha", 0);
        tw.onComplete(theTween =>
        {
            Game.instance.GoToPage(PageType.MenuPage);
        });
        Go.to(this, 2f, tw);

    }
}