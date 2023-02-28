using UnityEngine;
using System.Collections;

public class creatureBehaviour : MonoBehaviour {
    protected DungeonGenerator mapRef;
    protected GameObject playerRef;
    protected PlayerControls playerScriptRef;

    protected byte power;
    protected byte agility;
    protected byte resilliance;
    protected byte thought;
    protected byte spirit;

    protected byte maxHealth;
    protected short health;
    protected byte maxMana;
    protected byte mana;

    protected byte armor = 0;
    protected byte dodge;

    protected uint experianceValue;

    protected int temp;
    protected float tempf;
    protected Vector3 tempVector;
    protected float activeDistance;
    protected float detectedDistance;
    protected float attackDistance;
    // Use this for initialization
    void Start ()
    {
	
	}
    void Awake()
    {
        initialiseStats();
        GameObject linkToMap = GameObject.Find("LevelGenerator");
        mapRef = (DungeonGenerator)linkToMap.GetComponent(typeof(DungeonGenerator));

        playerRef = GameObject.Find("Player");
        playerScriptRef = (PlayerControls)playerRef.GetComponent(typeof(PlayerControls));

        maxHealth = (byte)(5 * resilliance);
        health = maxHealth;

        maxMana = (byte)(5 * thought);
        mana = maxMana;

        dodge = (byte)(5 * agility);

    }

    virtual protected void initialiseStats()
    {
        power = 1;
        agility = 1;
        resilliance = 1;
        thought = 1;
        spirit = 1;

        experianceValue = 500;

        activeDistance = 6.0f;
        detectedDistance = 4.0f;
        attackDistance = 1.0f;
    }

    public void attacked(byte incomingDamage, byte incomingAccuracey)
    {
        Debug.Log("creature attacked");
        if (Random.Range(1, 101) >= (50 - (incomingAccuracey - dodge)))
        {
            Debug.Log("creature hit");
            if ((incomingDamage - armor) > 0)
            {
                health -= (short)(incomingDamage - armor);
                if (health < 0)
                {
                    health = 0;
                }
                Debug.Log("creature Health = " + health);
            }
        }
    }
    public void giveExpValue()
    {
        playerScriptRef.gainExperiance(experianceValue);
    }
    protected bool moveLeft()
    {
        if (mapRef.checkDestination((int)transform.transform.position.x - 1, (int)transform.transform.position.y))
        {
            transform.Translate(-1.0f, 0.0f, 0.0f);
            return true;
        }
        else
            return false;
    }
    protected bool moveRight()
    {
        if (mapRef.checkDestination((int)transform.transform.position.x + 1, (int)transform.transform.position.y))
        {
            transform.Translate(1.0f, 0.0f, 0.0f);
            return true;
        }
        else
            return false;
    }
    protected bool moveUp()
    {
        if (mapRef.checkDestination((int)transform.transform.position.x, (int)transform.transform.position.y + 1))
        {
            transform.Translate(0.0f, 1.0f, 0.0f);
            return true;
        }
        else
            return false;
    }
    protected bool moveDown()
    {
        if (mapRef.checkDestination((int)transform.transform.position.x, (int)transform.transform.position.y - 1))
        {
            transform.Translate(0.0f, -1.0f, 0.0f);
            return true;
        }
        else
            return false;
    }
    public short reportHealth()
    {
        return health;
    }
    public virtual void Turn()
    {
        tempf = Vector2.Distance(playerRef.transform.transform.position, transform.transform.position);
        if (tempf <= activeDistance)
        {
            if (tempf <= detectedDistance)
            {
                if (tempf <= attackDistance)
                {
                    playerScriptRef.attacked(power, agility);
                }
                else
                    moveToPlayer();
            }
            else
            {
                wander();
            }
        } 
    }

    protected void moveToPlayer()
    {
        tempVector = new Vector3(playerRef.transform.transform.position.x - transform.transform.position.x, playerRef.transform.transform.position.y - transform.transform.position.y, 0.0f);
        if (Mathf.Abs(tempVector.x) > Mathf.Abs(tempVector.y)) // if X axis movement required is greater required than Y Axix, prioritise X Axis
        {
            if (tempVector.x > 0)
            {
                if (!moveRight())
                {
                    if (tempVector.y > 0)
                        moveUp();
                    else if (tempVector.y < 0)
                        moveDown();
                    else
                        wander();
                }
            }
            else
            {
                if (!moveLeft())
                {
                    if (tempVector.y > 0)
                        moveUp();
                    else if (tempVector.y < 0)
                        moveDown();
                    else
                        wander();
                }
            }
        }
        else
        {
            if (tempVector.y > 0)
            {
                if (!moveUp())
                {
                    if (tempVector.x > 0)
                        moveRight();
                    else if (tempVector.x < 0)
                        moveLeft();
                    else
                        wander();

                }
            }
            else
            {
                if (!moveDown())
                {
                    if (tempVector.x > 0)
                        moveRight();
                    else if (tempVector.x < 0)
                        moveLeft();
                    else
                        wander();

                }
            }
        }

    }

    protected void moveToAlly()
    {

    }

    protected void wander()
    {
        temp = (int)Random.Range(0, 4);
        byte attempts = 0;
        while (attempts < 4)
        {
            if (temp == 0)
                if (moveRight())
                    break;
                else
                {
                    attempts++;
                    temp += 1;
                }

            if (temp == 1)
                if (moveLeft())
                    break;
                else
                {
                    temp += 1;
                    attempts++;
                }
            if (temp == 2)
                if (moveUp())
                    break;
                else
                {
                    temp += 1;
                    attempts++;
                }
            if (temp == 3)
                if (moveDown())
                    break;
                else
                {
                    temp = 0;
                    attempts++;
                }
        }
    }

	// Update is called once per frame
	void Update ()
    {
	
	}
}
