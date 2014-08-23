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
    List<Vector2> itemSpawns;
    Vector2 mapSize;
	public LevelData(){

	}

    public void LevelDataXML(string pathToXml)
    {
        parseFile = Resources.Load(pathToXml) as TextAsset;
        platforms = new List<PlatformData>();
        metadata = new List<MetaData>();
        itemSpawns = new List<Vector2>();
        playerSpawns = new List<Vector2>();
        enemySpawns = new List<Vector2>();
        XmlTextReader mapReader = new XmlTextReader(new StringReader(parseFile.text));
        mapReader.Read();
        XmlDocument doc = new XmlDocument();
        doc.LoadXml(mapReader.ReadOuterXml());
        mapSize = new Vector2((float)int.Parse(doc.ChildNodes[0].Attributes[0].Value)/12, (float)int.Parse(doc.ChildNodes[0].Attributes[1].Value)/12);
        for (int x = 0; x < doc.ChildNodes[0].ChildNodes.Count; x++)
        {
            if (doc.ChildNodes[0].ChildNodes[x].Name.ToLower().Equals("frontlayer")) {
                LoadTiles(doc.ChildNodes[0].ChildNodes[x], platforms);
            }
            if (doc.ChildNodes[0].ChildNodes[x].Name.ToLower().Equals("backgroundlayer"))
            {
                
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

            if (spawns.ChildNodes[x].Name.ToLower().Equals("playerspawn")) {
                playerSpawns.Add(new Vector2(int.Parse(spawns.ChildNodes[x].Attributes[1].Value)/12, int.Parse(spawns.ChildNodes[x].Attributes[2].Value)/12));
            }
            if (spawns.ChildNodes[x].Name.ToLower().Equals("enemyspawn"))
            {
                enemySpawns.Add(new Vector2(int.Parse(spawns.ChildNodes[x].Attributes[1].Value) / 12, int.Parse(spawns.ChildNodes[x].Attributes[2].Value) / 12));
            }
            if (spawns.ChildNodes[x].Name.ToLower().Equals("itemspawn"))
            {
                itemSpawns.Add(new Vector2(int.Parse(spawns.ChildNodes[x].Attributes[1].Value) / 12, int.Parse(spawns.ChildNodes[x].Attributes[2].Value) / 12));
            }
        }
    }

	private void setPlatformData(IList platform_list){
		foreach (object platform in platform_list) {
			IDictionary temp_platform = (IDictionary)platform;
			double temp_x = (double) temp_platform ["x"] ;
			float x = (float)temp_x;
			double temp_y = (double) temp_platform ["y"] ;
			float y = (float)temp_y;
			string image = (string)temp_platform ["image"];
			platforms.Add (new PlatformData(x,y,image));
		}
	}

    private void setPlayerData(IList platform_list)
    {
        foreach (object platform in platform_list)
        {
            IDictionary temp_platform = (IDictionary)platform;
            double temp_x = (double)temp_platform["x"];
            float x = (float)temp_x;
            double temp_y = (double)temp_platform["y"];
            float y = (float)temp_y;
            playerSpawns.Add(new Vector2(x,y));
        }
    }

    private void setEnemyData(IList enemy_list)
    {
        foreach (object enemy in enemy_list)
        {
            IDictionary temp_enemy = (IDictionary)enemy;
            double temp_x = (double)temp_enemy["x"];
            float x = (float)temp_x;
            double temp_y = (double)temp_enemy["y"];
            float y = (float)temp_y;
            enemySpawns.Add(new Vector2(x, y));
        }
    }

    private void setItemData(IList platform_list)
    {
        foreach (object platform in platform_list)
        {
            IDictionary temp_platform = (IDictionary)platform;
            double temp_x = (double)temp_platform["x"];
            float x = (float)temp_x;
            double temp_y = (double)temp_platform["y"];
            float y = (float)temp_y;
            itemSpawns.Add(new Vector2(x,y));
        }
    }


	public List<PlatformData> getPlatformData(){
		return platforms;
	}

    public List<Vector2> getPlayerSpawns()
    {
        return playerSpawns;
    }

    public List<Vector2> getItemSpawns()
    {
        return itemSpawns;
    }

    public List<Vector2> getEnemySpawns()
    {
        return enemySpawns;
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