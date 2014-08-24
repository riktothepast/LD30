using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.IO;

public class LevelData {

	TextAsset parseFile;
	List<PlatformData> platforms;
    List<PlatformData> platformsBackground;
    List<MetaData> metadata;
    List<Vector2> playerSpawns;
    List<Vector2> enemySpawns;
    List<Vector2> items;
    List<Portal> portalSpawns;
    List<Hazard> hazardSpawns;
    Vector2 mapSize;
	public LevelData(){

	}

    public void LevelDataXML(string pathToXml)
    {
        parseFile = Resources.Load(pathToXml) as TextAsset;
        platforms = new List<PlatformData>();
        metadata = new List<MetaData>();
        portalSpawns = new List<Portal>();
        hazardSpawns = new List<Hazard>();
        playerSpawns = new List<Vector2>();
        enemySpawns = new List<Vector2>();
        items = new List<Vector2>();
        XmlTextReader mapReader = new XmlTextReader(new StringReader(parseFile.text));
        mapReader.Read();
        XmlDocument doc = new XmlDocument();
        doc.LoadXml(mapReader.ReadOuterXml());
        mapSize = new Vector2((float)int.Parse(doc.ChildNodes[0].Attributes[0].Value)/16, (float)int.Parse(doc.ChildNodes[0].Attributes[1].Value)/16);
        for (int x = 0; x < doc.ChildNodes[0].ChildNodes.Count; x++)
        {
            if (doc.ChildNodes[0].ChildNodes[x].Name.ToLower().Equals("frontlayer")) {
                LoadTiles(doc.ChildNodes[0].ChildNodes[x], platforms);
            }
            if (doc.ChildNodes[0].ChildNodes[x].Name.ToLower().Equals("entities"))
            {
                LoadSpawnPoints(doc.ChildNodes[0].ChildNodes[x]);
            }
        }
    }

    void LoadTiles(XmlNode FGTiles, List<PlatformData> toSave) {
        
        string tileSet="";
        for (int x = 0; x < FGTiles.Attributes.Count; x++)
            if(FGTiles.Attributes[x].Name.ToLower().Equals("tileset"))
                tileSet = FGTiles.Attributes[x].Value+"_";
        // split in lines
        string[] data = FGTiles.InnerText.Split('\n');
        //go one by one :D
        for (int dataControl = 0; dataControl < data.Length; dataControl++)
        {
            string[] line = data[dataControl].Split(',');
            for (int lineControl = 0; lineControl < line.Length; lineControl++)
            {
                if (int.Parse(line[lineControl])>=0) {
                    toSave.Add(new PlatformData(lineControl, dataControl, (tileSet + line[lineControl]).Trim()));
                }
            }
        }
    }

    void LoadSpawnPoints(XmlNode spawns) {
        for (int x = 0; x < spawns.ChildNodes.Count; x++) {

            if (spawns.ChildNodes[x].Name.ToLower().Equals("playerspawn"))
            {
                playerSpawns.Add(new Vector2(int.Parse(spawns.ChildNodes[x].Attributes[1].Value), -int.Parse(spawns.ChildNodes[x].Attributes[2].Value)));
            }
            if (spawns.ChildNodes[x].Name.ToLower().Equals("enemyspawn"))
            {
                enemySpawns.Add(new Vector2(int.Parse(spawns.ChildNodes[x].Attributes[1].Value) / 16, int.Parse(spawns.ChildNodes[x].Attributes[2].Value) / 16));
            }
            if (spawns.ChildNodes[x].Name.ToLower().Equals("portals"))
            {
                int dest = 0;
                for (int t = 3; t < 9; t++ )
                {
                    if (spawns.ChildNodes[x].Attributes[t].Value!="0")
                    {
                        dest = int.Parse(spawns.ChildNodes[x].Attributes[t].Value) - 1;
                    }
                }
                portalSpawns.Add(new Portal(new Vector2(int.Parse(spawns.ChildNodes[x].Attributes[1].Value), -int.Parse(spawns.ChildNodes[x].Attributes[2].Value)), dest));
            }
            if (spawns.ChildNodes[x].Name.ToLower().Equals("hazards"))
            {
                hazardSpawns.Add(new Hazard(new Vector2(int.Parse(spawns.ChildNodes[x].Attributes[1].Value), -int.Parse(spawns.ChildNodes[x].Attributes[2].Value))));
            }

            if (spawns.ChildNodes[x].Name.ToLower().Equals("enemies"))
            {
                enemySpawns.Add(new Vector2(int.Parse(spawns.ChildNodes[x].Attributes[1].Value), -int.Parse(spawns.ChildNodes[x].Attributes[2].Value)));
            }

            if (spawns.ChildNodes[x].Name.ToLower().Equals("backpacks"))
            {
                items.Add(new Vector2(int.Parse(spawns.ChildNodes[x].Attributes[1].Value), -int.Parse(spawns.ChildNodes[x].Attributes[2].Value)));
            }

        }
    }

	public List<PlatformData> getPlatformData(){
		return platforms;
	}

    public List<Vector2> getPlayerSpawns()
    {
        return playerSpawns;
    }

    public List<Portal> getPortalSpawns()
    {
        return portalSpawns;
    }

    public List<Hazard> getHazardSpawns()
    {
        return hazardSpawns;
    }

    public List<Vector2> getEnemySpawns()
    {
        return enemySpawns;
    }

    public List<Vector2> getItemSpawns()
    {
        return items;
    }

    public List<MetaData> getMetaData()
    {
        return metadata;
    }

    public Vector2 getTileMapSize() {
        return mapSize;
    }

}

public class MetaData {
    public string map_name;
    public int width;
    public int height;

    public MetaData(float w, float h, string n)
		{
            this.width = (int)w;
			this.height = (int)h;
			this.map_name = n;
		}
}