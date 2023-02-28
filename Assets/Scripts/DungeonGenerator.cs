using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DungeonGenerator : MonoBehaviour {
	public GameObject wall;
    public GameObject floor;
	public GameObject[] creaturePrefab;
    public GameObject[] itemPrefab;
	public GameObject exitPrefab;
    public List <GameObject> world = new List<GameObject>();
    public List <GameObject> items = new List<GameObject>();
    public List <GameObject> creatures = new List<GameObject>();
	public float dungeonXsize;
	public float dungeonYsize;
	public int buildingBlockSize = 1;
	private bool[,] mapping;
    private int level = 1;
	private int features = 0;
	public GameObject player;
	public GameObject exit;
    public GameObject settingsObject;
    private settingsStorageScript settingScript;
    public AudioSource backgroundMusic;
    AudioClip backgroundMusicAudioClip;
    private float overallMusicVolume;
	private int attempts = 0;
    private Vector3 temp;
    void Awake()
    {
        player = GameObject.Find("Player");
        settingsObject = GameObject.Find("PersistantSettingsObject");
        settingScript = (settingsStorageScript)settingsObject.GetComponent(typeof(settingsStorageScript));
        Debug.Log("Master volume set to: " + settingScript.getMasterVolume());
        Debug.Log("Music volume set to: " + settingScript.getMusicVolume());
        Debug.Log("SFX volume set to: " + settingScript.getEffectVolume());
        backgroundMusic = gameObject.AddComponent<AudioSource>();
        backgroundMusicAudioClip = (AudioClip)Resources.Load("Sound/SteelForHumans");
        //AudioSource.PlayClipAtPoint(backgroundMusicAudioClip, transform.position, ((float)(settingScript.getMusicVolume() / 100) * (float)(settingScript.getMasterVolume() / 100)));
        backgroundMusic.clip = backgroundMusicAudioClip;
        backgroundMusic.loop = true;
        backgroundMusic.volume = ((float)(settingScript.getMusicVolume()) / 100) * ((float)(settingScript.getMasterVolume()) / 100);
    }
    void Start ()
	{
        backgroundMusic.Play();
        Instantiate(floor, new Vector3((dungeonXsize / 2), (dungeonYsize / 2), 1.0f), Quaternion.identity);
        floor.transform.localScale = new Vector3(dungeonXsize * buildingBlockSize, dungeonYsize * buildingBlockSize, 1.0f);
        //random dungeon generation
        mapping = new bool[(int)dungeonXsize,(int)dungeonYsize];
		wall.transform.localScale = new Vector3(1.0f * buildingBlockSize, 1.0f * buildingBlockSize, 1.0f);
		GenerateWorld();
		for(int y = 0; y <= dungeonYsize-1; y++)
		{
			for (int x = 0; x <= dungeonXsize-1; x++)
			{
				if(mapping[x,y] == false)
				{
					world.Add((GameObject)Instantiate(wall, new Vector3(x * buildingBlockSize, y * buildingBlockSize, 0.0f), Quaternion.identity));
				}
			}
		}
		insertExit ();
		insertPlayer();
        for (int i = 0; i < Random.Range(1, 10); i++)
        {
            insertCreature();
        }
        for (int i = 0; i < Random.Range(1, 10); i++)
        {
            insertItems();
        }
    }

    public void npcTurn()
    {
       for (int i = 0; i < creatures.Count; i++)
       {
            creatureBehaviour temp = (creatureBehaviour)creatures[i].gameObject.GetComponent(typeof(creatureBehaviour));
            if (temp.reportHealth() <= 0)
            {
                temp.giveExpValue();
                Destroy(creatures[i].gameObject);
                creatures.RemoveAt(i);
            }
            else
            {
                temp.Turn();
            }
        }
       for (int i = 0; i< items.Count; i++)
       {
            itemScript temp = (itemScript)items[i].gameObject.GetComponent(typeof(itemScript));
            if(items[i].transform.transform.position == player.transform.transform.position)
            {
                temp.itemAction();
                Destroy(items[i].gameObject);
                items.RemoveAt(i);
            }
       }
    }

    public void randomPlacement()
    {
        bool Placed = false;
        int randX = 0;
        int randY = 0;
        while (!Placed)
        {
            randX = (int)(Random.Range(1.0f, dungeonXsize - 1));
            randY = (int)(Random.Range(1.0f, dungeonYsize - 1));
            if (mapping[randX, randY] == true)
            {
                Placed = true;
            }
            else if (mapping[randX + 1, randY] == true)
            {
                Placed = true;
                randX += 1;
            }
            else if (mapping[randX - 1, randY] == true)
            {
                Placed = true;
                randX -= 1;
            }
            else if (mapping[randX, randY + 1] == true)
            {
                Placed = true;
                randY += 1;
            }
            else if (mapping[randX, randY - 1] == true)
            {
                Placed = true;
                randY -= 1;
            }
        }
        temp = new Vector3((float)randX * buildingBlockSize, (float)randY * buildingBlockSize, 0.0f);
    }

	public void requestNewLevel() 
	{
        Destroy(exit.gameObject);
        features = 0;
        attempts = 0;
        level++;

        //Clear bool array containing world data
        for (int i = mapping.GetLength(1) - 1; i > 0; i--)
        {
            for (int j = mapping.GetLength(0) - 1; j > 0; j--)
            {
                mapping[i, j] = false;
            }
        }

        for (int i = 0; i < world.Count; i++)//destroy clones of walls displaying world
        {
            Destroy(world[i].gameObject);
            Debug.Log("Destroyed object " + i);
        }
        world.Clear();//ensure list is cleared

        for (int i = 0; i < creatures.Count; i++)
        {
            Destroy(creatures[i].gameObject);
            Debug.Log("Destroyed creature " + i);
        }
        creatures.Clear();
        for (int i = 0; i < items.Count; i++)
        {
            Destroy(items[i].gameObject);
        }
        items.Clear();

        GenerateWorld();//create new world data
        for (int y = 0; y <= dungeonYsize - 1; y++)
        {
            for (int x = 0; x <= dungeonXsize - 1; x++)
            {
                if (mapping[x, y] == false)
                {
                    world.Add((GameObject)Instantiate(wall, new Vector3(x * buildingBlockSize, y * buildingBlockSize, 0.0f), Quaternion.identity));
                }
            }
        }

        insertPlayer();
        insertExit();
        for (int i = 0; i < Random.Range(1, 10); i++)
        {
            insertCreature();
        }
        for (int i = 0; i < Random.Range(1, 10); i++)
        {
            insertItems();
        }
    }

	public bool[,] get2DMap()
	{
		return mapping;
	}


    public bool checkDestination(int x, int y)
    {
        if((int)player.transform.transform.position.x == x && (int)player.transform.transform.position.y == y)// check that player is not in that possition
        {
            return false;
        }
        for(int i = 0; i < creatures.Count; i++)
        {
            if((int)creatures[i].transform.transform.position.x == x && (int)creatures[i].transform.transform.position.y == y)
            {
                return false;
            }
        }
        return mapping[x,y];
    }
    public Vector3 getNearestCreature(int x, int y)
    {
        Vector3 temp = new Vector3(x, y, 0);
        Vector3 origen = new Vector3(x,y,0);
        int closestCreature = 0;
        float distance = float.MaxValue;

        if (creatures.Count > 1)//if creature is only remaining creature on level return it's own locatation
        {
            for (int i = 0; i < creatures.Count; i++)
            {
                if (Vector3.Distance(origen, creatures[i].transform.transform.position) <= distance)
                {
                    closestCreature = i;
                    distance = Vector3.Distance(origen, creatures[i].transform.transform.position);
                }
            }
            temp = creatures[closestCreature].transform.transform.position;
        }
        return temp;
    }

    public bool attackCreature(byte incomingDamage, byte incomingAccuracey, int x, int y)
    {
        for(int i = 0; i < creatures.Count; i++)
        {
            if ((int)creatures[i].transform.transform.position.x == x && (int)creatures[i].transform.transform.position.y == y)
            {
                creatureBehaviour temp = (creatureBehaviour)creatures[i].gameObject.GetComponent(typeof(creatureBehaviour));
                temp.attacked(incomingDamage, incomingAccuracey);
                return true;
            }
        }
        return false;
    }
	public GameObject getPlayer()
	{
		return player;
	}
	public int getBlockSize()
	{
		return buildingBlockSize;
	}

	void insertPlayer()
	{
        randomPlacement();
        player.transform.transform.position = temp;
    }
	void insertCreature()
	{
        randomPlacement();
        if (player.transform.transform.position == temp)
        {
            randomPlacement();
        }
        if(level <= creaturePrefab.Length)
            creatures.Add((GameObject)Instantiate(creaturePrefab[Random.Range(0, level)], new Vector3(temp.x, temp.y, -1.0f), Quaternion.identity));
        else
            creatures.Add((GameObject)Instantiate(creaturePrefab[Random.Range(0, creaturePrefab.Length)], new Vector3(temp.x, temp.y, -1.0f), Quaternion.identity));
    }
    void insertItems()
    {
        randomPlacement();
        if (player.transform.transform.position == temp)
        {
            randomPlacement();
        }
        items.Add((GameObject)Instantiate(itemPrefab[Random.Range(0, itemPrefab.Length)], temp, Quaternion.identity));
    }

    void insertExit()
	{
        randomPlacement();
        exit = (GameObject)Instantiate(exitPrefab, temp, Quaternion.identity);
    }

	void generateFirstRoom(int x, int y)
	{
		bool invalidRoom = false;
		
		//check that room is inside of map  not attached to more than 2 other features.
		for (int i = y-2; i <= y+2; i++)
		{
			for(int j = x-2; j <= x+2; j++)
			{
				if(i < 0 || i > dungeonXsize-1)
				{
					invalidRoom = true;
					break;
				}
				else if(j < 0 || j > dungeonYsize-1)
				{
					invalidRoom = true;
					break;
				}
			}
		}
		if(!invalidRoom)
		{
			mapping[x, y] = true;
			mapping[x, y+1] = true;
			mapping[x, y-1] = true;
            
			mapping[x+1, y] = true;
			mapping[x+1, y+1] = true;
			mapping[x+1, y-1] = true;
            
			mapping[x-1, y] = true;
			mapping[x-1, y+1] = true;
			mapping[x-1, y-1] = true;
            features++;
        }
    }

	void generateRoom(int x, int y)
	{
		int attachmentCount = 0;
		bool invalidRoom = false;
		
		//check that room is inside of map  not attached to more than 2 other features.
		for (int i = y-2; i <= y+2; i++)
		{
			for(int j = x-2; j <= x+2; j++)
			{
				if(i < 0 || i > dungeonYsize-1)
				{
					invalidRoom = true;
					break;
				}
				else if(j < 0 || j > dungeonXsize-1)
				{
					invalidRoom = true;
					break;
				}
			}
		}
		if(!invalidRoom)
		{
			if(mapping[x+2, y] == true)
			{
				attachmentCount++;
			}
			if(mapping[x-2, y] == true)
			{
				attachmentCount++;
			}
			if(mapping[x, y+2] == true)
			{
				attachmentCount++;
			}
			if(mapping[x, y-2] == true)
			{
				attachmentCount++;
			}
			if(mapping[x+1, y-2] == true)
			{
				attachmentCount++;
			}
			if(mapping[x+1, y+2] == true)
			{
				attachmentCount++;
			}
			if(mapping[x-1, y-2] == true){
				attachmentCount++;
			}
			if(mapping[x-1, y+2] == true)
			{
				attachmentCount++;
			}
			if(mapping[x+2, y-1] == true)
			{
				attachmentCount++;
			}
			if(mapping[x+2, y+1] == true)
			{
				attachmentCount++;
			}
			if(mapping[x-2, y-1] == true)
			{
				attachmentCount++;
			}
			if(mapping[x-2, y+1] == true)
			{
				attachmentCount++;
			}
			
			
		}
		if(!invalidRoom && attachmentCount < 3 && attachmentCount > 0)
		{
			mapping[x, y] = true;
			mapping[x, y+1] = true;
			mapping[x, y-1] = true;
			
			mapping[x+1, y] = true;
			mapping[x+1, y+1] = true;
			mapping[x+1, y-1] = true;
			
			mapping[x-1, y] = true;
			mapping[x-1, y+1] = true;
			mapping[x-1, y-1] = true;
			features++;
			attempts = 0;
		}
	}

	private void generateCorridor(int x, int y)
	{
		int attachmentCount = 0;
		bool invalidRoom = false;
		bool direction = (bool)(Random.Range(0.0f, 1.0f) < 0.5);
		if(direction)
		{			
			//check that corridor is inside of map  not attached to more than 2 other features.
			for (int i = y-2; i <= y+2; i++)
			{
				for(int j = x-1; j <= x+1; j++)
				{
					if(i < 0 || i > dungeonXsize-1)
					{
						invalidRoom = true;
						break;
					}
					else if(j < 0 || j > dungeonYsize-1)
					{
						invalidRoom = true;
						break;
					}
				}
			}
			if(!invalidRoom)
			{
				if(mapping[x, y+2] == true)
				{
					attachmentCount++;
				}
				if(mapping[x, y-2] == true)
				{
					attachmentCount++;
				}
				if(mapping[x+1, y+1] == true)
				{
					attachmentCount++;
				}
				if(mapping[x+1, y-1] == true)
				{
					attachmentCount++;
				}
				if(mapping[x-1, y+1] == true)
				{
					attachmentCount++;
				}
				if(mapping[x-1, y-1] == true)
				{
					attachmentCount++;
				}
				
			}
			if(!invalidRoom && attachmentCount < 3 && attachmentCount > 0)
			{
				mapping[x, y] = true;
				mapping[x, y+1] = true;
				mapping[x, y-1] = true;
				attempts = 0;
			}
		}
		else
		{
			//check that corridor is inside of map  not attached to more than 2 other features.
			for (int i = y-1; i <= y+1; i++)
			{
				for(int j = x-2; j <= x+2; j++)
				{
					if(i < 0 || i >= dungeonXsize-2)
					{
						invalidRoom = true;
						break;
					}
					else if(j < 0 || j >= dungeonYsize-1)
					{
						invalidRoom = true;
						break;
					}
				}
			}
			if(!invalidRoom)
			{
				if(mapping[x+2, y] == true)
				{
					attachmentCount++;
				}
				if(mapping[x-2, y] == true)
				{
					attachmentCount++;
				}
				if(mapping[x+1, y+1] == true)
				{
					attachmentCount++;
				}
				if(mapping[x+1, y-1] == true)
				{
					attachmentCount++;
				}
				if(mapping[x-1, y+1] == true)
				{
					attachmentCount++;
				}
				if(mapping[x-1, y-1] == true)
				{
					attachmentCount++;
				}			
			}
			if(!invalidRoom && attachmentCount < 3 && attachmentCount > 0)
			{
				mapping[x, y] = true;
				mapping[x+1, y] = true;
				mapping[x-1, y] = true;
				attempts = 0;
			}
		}
	}

	private void GenerateWorld()
	{
		//Generate central chamber in the world. 3x3 room.		 
		generateFirstRoom((int)dungeonXsize/2, (int)dungeonYsize/2);
		while(features <= (dungeonXsize + dungeonYsize) && attempts <  (dungeonXsize * dungeonYsize))
		{
			bool featureType = (bool)(Random.Range(0.0f, 1.0f) < 0.5);
			
			int randX = (int)(Random.Range(0.0f, dungeonXsize - 1) + 1);
			int randY = (int)(Random.Range(0.0f, dungeonYsize - 1) + 1);
			if(featureType)
			{
				generateRoom((int)randX, (int)randY);
			}
			else
			{
				generateCorridor((int)randX, (int)randY);
			}
			attempts++;
		}
		
	}
}