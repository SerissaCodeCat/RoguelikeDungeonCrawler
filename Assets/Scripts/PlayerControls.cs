using UnityEngine;
using UnityEngine.UI;
using image = UnityEngine.UI.Image;
using System.Collections;

public class PlayerControls : MonoBehaviour {
    private DungeonGenerator mapRef;
    private GameObject exitRef;
    private image healthBar;
    private image staminaBar;
    private image manaBar;

    private Text levelTextRef;
    private Text powerTextRef;
    private Text agilityTextRef;
    private Text resillianceTextRef;
    private Text thoughtTextRef;
    private Text spiritTextRef;
    private Text statPointTextRef;
    private Text currentExpTextRef;

    private Text healthPotionCount;
    private Text staminaPotionCount;
    private Text manaPotionCount;


    private Button powerUp;
    private Button agilityUp;
    private Button resillianceUp;
    private Button thoughtUp;
    private Button spiritUp;

    private Button useHealthPotionButton;
    private Button useStaminaPotionButton;
    private Button useManaPotionButton;

    private Button useFireballButton;
    private Button useMageArmorButton;
    private Button useHealSpellButton;



    protected bool isAttacking = false;
    protected bool isDeffending = false;

    protected byte atributePoints = 5;
    protected byte power = 1;
    protected byte agility = 1;
    protected byte resilliance = 1;
    protected byte thought = 1;
    protected byte spirit = 1;

    protected byte healthPotion = 0;
    protected byte manaPotion = 0;
    protected byte staminaPotion = 0;

    protected float maxHealth;
    protected float health;
    protected float maxMana;
    protected float mana;
    protected float maxStamina;
    protected float stamina;

    protected short armor = 0;
    protected short mageArmorValue = 0;
    protected short armorSpellCount;
    protected short dodge;

    private uint experiance;
    private uint levelupPoint;
    private short playerLevel;
    // Use this for initialization
    void Awake ()
    {
        GameObject linkToMap = GameObject.Find("LevelGenerator");
        exitRef = GameObject.FindGameObjectWithTag("Finish");
        mapRef = (DungeonGenerator)linkToMap.GetComponent(typeof(DungeonGenerator));

        healthBar = GameObject.Find("healthBar").GetComponent<image>();
        staminaBar = GameObject.Find("staminaBar").GetComponent<image>();
        manaBar = GameObject.Find("manaBar").GetComponent<image>();

        levelTextRef = GameObject.Find("levelText").GetComponent<Text>();
        powerTextRef = GameObject.Find("powerText").GetComponent<Text>();
        agilityTextRef = GameObject.Find("agilityText").GetComponent<Text>();
        resillianceTextRef = GameObject.Find("resillianceText").GetComponent<Text>();
        thoughtTextRef = GameObject.Find("thoughtText").GetComponent<Text>();
        spiritTextRef = GameObject.Find("spiritText").GetComponent<Text>();
        statPointTextRef = GameObject.Find("statPointText").GetComponent<Text>();
        currentExpTextRef = GameObject.Find("currentExpText").GetComponent<Text>();

        powerUp = GameObject.Find("powerButton").GetComponent<Button>();
        agilityUp = GameObject.Find("agilityButton").GetComponent<Button>();
        resillianceUp = GameObject.Find("resillianceButton").GetComponent<Button>();
        thoughtUp = GameObject.Find("thoughtButton").GetComponent<Button>();
        spiritUp = GameObject.Find("spiritButton").GetComponent<Button>();

        healthPotionCount = GameObject.Find("healthPotions").GetComponent<Text>();
        staminaPotionCount = GameObject.Find("staminaPotions").GetComponent<Text>();
        manaPotionCount = GameObject.Find("manaPotions").GetComponent<Text>();

        useHealthPotionButton = GameObject.Find("healthPotionButton").GetComponent<Button>();
        useStaminaPotionButton = GameObject.Find("staminaPotionButton").GetComponent<Button>();
        useManaPotionButton = GameObject.Find("manaPotionButton").GetComponent<Button>();

        useFireballButton = GameObject.Find("fireballButton").GetComponent<Button>();
        useMageArmorButton = GameObject.Find("mageArmorButton").GetComponent<Button>();
        useHealSpellButton = GameObject.Find("healSpellButton").GetComponent<Button>();

        useHealthPotionButton.interactable = false;
        useStaminaPotionButton.interactable = false;
        useManaPotionButton.interactable = false;

        useFireballButton.interactable = false;
        useMageArmorButton.interactable = false;
        useHealSpellButton.interactable = false;


        maxHealth =  5 * resilliance;
        health = maxHealth;

        maxMana = 5 * thought;
        mana = maxMana;

        maxStamina = 5 * power;
        stamina = maxStamina;


        dodge = (short)(5 * agility);
        playerLevel = 1;
        experiance = 0;
        levelupPoint = (uint)(playerLevel * 1000);

        levelTextRef.text = playerLevel.ToString();
        powerTextRef.text = power.ToString();
        agilityTextRef.text = agility.ToString();
        resillianceTextRef.text = resilliance.ToString();
        thoughtTextRef.text = thought.ToString();
        spiritTextRef.text = spirit.ToString();
        statPointTextRef.text = atributePoints.ToString();
        currentExpTextRef.text = (experiance.ToString() + " / " + levelupPoint.ToString() + " exp");
        setSpellButtons();

    }
    private void endTurn()
    {
        if(armorSpellCount > 0)
        {
            armorSpellCount--;
            if(armorSpellCount == 0)
            {
                mageArmorValue = 0;
                setSpellButtons();
            }
        }
        mapRef.npcTurn();
    }
    public void gainExperiance(uint exp)
    {
        if (experiance + exp <= uint.MaxValue) // check max level not reached.
        {
            experiance += exp;
            Debug.Log("gained " + exp + " Exp!");
            if (experiance >= levelupPoint)
            {
                playerLevel++;
                levelupPoint += (uint)(playerLevel * 1000);
                atributePoints++;
                Debug.Log("GAILED LEVEL, player is level " + playerLevel);
                levelTextRef.text = playerLevel.ToString();
                statPointTextRef.text = atributePoints.ToString();
                alterStatButtons();
            }
            currentExpTextRef.text = (experiance.ToString() + " / " + levelupPoint.ToString() + " exp");
        }
    }
    private void alterStatButtons()
    {
        if(atributePoints == 0)
        {
            powerUp.interactable = false;
            agilityUp.interactable = false;
            resillianceUp.interactable = false;
            thoughtUp.interactable = false;
            spiritUp.interactable = false;
        }
        else
        {
            powerUp.interactable = true;
            agilityUp.interactable = true;
            resillianceUp.interactable = true;
            thoughtUp.interactable = true;
            spiritUp.interactable = true;
        }
    }
    public byte getPower()
    {
        return power;
    }
    public void increasePower()
    {
        if (atributePoints > 0)
        {
            if (power + 1 != byte.MaxValue)
            {
                power++;
                maxStamina = (5 * power);
                stamina += 5; // each point of power increases stamina by 5, this keeps a full stamina bar full.
                staminaBar.fillAmount = (stamina / maxStamina);
                atributePoints--;
                statPointTextRef.text = atributePoints.ToString();
                powerTextRef.text = power.ToString();
                alterStatButtons();
            }
        }
    }

    public byte getAgility()
    {
        return agility;
    }
    public void increaseAgility()
    {
        if (atributePoints > 0)
        {
            if (agility + 1 != byte.MaxValue)
            {
                agility++;
                dodge = (short)(5 * agility);// increase player's dodge chance
                atributePoints--;
                statPointTextRef.text = atributePoints.ToString();
                agilityTextRef.text = agility.ToString();
                alterStatButtons();
            }
        }
    }

    public byte getResilliance()
    {
        return resilliance;
    }
    public void increaseResilliance()
    {
        if (atributePoints > 0)
        {
            if (resilliance + 1 != byte.MaxValue)
            {
                resilliance++;
                maxHealth = (5 * resilliance);
                health += 5; // each point of resilliance increases health by 5, this keeps a full health bar full.
                healthBar.fillAmount = (health / maxHealth);
                atributePoints--;
                statPointTextRef.text = atributePoints.ToString();
                resillianceTextRef.text = resilliance.ToString();
                alterStatButtons();
            }
        }
    }

    public byte getThought()
    {
        return thought;
    }
    public void increaseThought()
    {
        if (atributePoints > 0)
        {
            if (thought + 1 != byte.MaxValue)
            {
                thought++;
                maxMana = 5 * thought;
                mana += 5; // each point of thought increases mana by 5, this keeps a full mana bar full.
                manaBar.fillAmount = (mana / maxMana);
                atributePoints--;
                statPointTextRef.text = atributePoints.ToString();
                thoughtTextRef.text = thought.ToString();
                setSpellButtons();
                alterStatButtons();
            }
        }
    }

    public byte getSpirit()
    {
        return spirit;
    }
    public void increaseSpirit()
    {
        if (atributePoints > 0)
        {
            if (spirit + 1 != byte.MaxValue)
            {
                spirit++;
                atributePoints--;
                statPointTextRef.text = atributePoints.ToString();
                spiritTextRef.text = spirit.ToString();
                alterStatButtons();
            }
        }
    }

    public bool gainHealthPotion()
    {
        if(healthPotion +1 < byte.MaxValue)
        {
            healthPotion++;
            useHealthPotionButton.interactable = true;
            healthPotionCount.text = healthPotion.ToString();
            return true;
        }
        return false;
    } 
    public void useHealthPotion()
    {
        if(healthPotion > 0)
        {
            health += (maxHealth / 10);
            if (health > maxHealth)
            {
                health = maxHealth;
            }
            healthBar.fillAmount = (health / maxHealth);
            healthPotion--;
            healthPotionCount.text = healthPotion.ToString();
            endTurn();
        }
        if (healthPotion == 0)
        {
            useHealthPotionButton.interactable = false;
        }
    }


    public bool gainStaminaPotion()
    {
        if (staminaPotion + 1 < byte.MaxValue)
        {
            staminaPotion++;
            useStaminaPotionButton.interactable = true;
            staminaPotionCount.text = staminaPotion.ToString();
            return true;
        }
        return false;
    }

    public void usestaminaPotion()
    {
        if (staminaPotion > 0)
        {
            stamina += (maxStamina / 10);
            if (stamina > maxStamina)
            {
                stamina = maxStamina;
            }
            staminaBar.fillAmount = (stamina / maxStamina);
            staminaPotion--;
            staminaPotionCount.text = staminaPotion.ToString();
            endTurn();
        }
        if (staminaPotion == 0)
        {
            useStaminaPotionButton.interactable = false;
        }
    }

    public bool gainManaPotion()
    {
        if (manaPotion + 1 < byte.MaxValue)
        {
            manaPotion++;
            useManaPotionButton.interactable = true;
            manaPotionCount.text = manaPotion.ToString();
            return true;
        }
        return false;
    }
    public void useManaPotion()
    {
        if (manaPotion > 0)
        {
            mana += (maxMana / 10);
            if (mana > maxMana)
            {
                mana = maxMana;
            }
            manaBar.fillAmount = (mana / maxMana);
            manaPotion--;
            manaPotionCount.text = manaPotion.ToString();
            setSpellButtons();
            endTurn();
        }
        if (manaPotion == 0)
        {
            useManaPotionButton.interactable = false;
        }
    }

    public void useFireballSpell()
    {
        if(mana >= 15)
        {
            mapRef.attackCreature((byte)(5 * spirit), byte.MaxValue, (int)transform.transform.position.x + 1, (int)transform.transform.position.y);
            mapRef.attackCreature((byte)(5 * spirit), byte.MaxValue, (int)transform.transform.position.x + 1, (int)transform.transform.position.y+1);
            mapRef.attackCreature((byte)(5 * spirit), byte.MaxValue, (int)transform.transform.position.x + 1, (int)transform.transform.position.y-1);

            mapRef.attackCreature((byte)(5 * spirit), byte.MaxValue, (int)transform.transform.position.x - 1, (int)transform.transform.position.y);
            mapRef.attackCreature((byte)(5 * spirit), byte.MaxValue, (int)transform.transform.position.x - 1, (int)transform.transform.position.y+1);
            mapRef.attackCreature((byte)(5 * spirit), byte.MaxValue, (int)transform.transform.position.x - 1, (int)transform.transform.position.y-1);

            mapRef.attackCreature((byte)(5 * spirit), byte.MaxValue, (int)transform.transform.position.x, (int)transform.transform.position.y+1);
            mapRef.attackCreature((byte)(5 * spirit), byte.MaxValue, (int)transform.transform.position.x, (int)transform.transform.position.y-1);

            mana -= 15;
            manaBar.fillAmount = (mana / maxMana);
            setSpellButtons();
            endTurn();
        }
        else
            setSpellButtons();
    }
    public void useArmorSpell()
    {
        if(mana >= 10)
        {
            mageArmorValue = (short)(5 * spirit);
            armorSpellCount = 10;
            mana -= 10;
            manaBar.fillAmount = (mana / maxMana);
            setSpellButtons();
            endTurn();
        }
        else
            setSpellButtons();
    }
    public void useHealSpell()
    {
        if(mana >= 5)
        {
            health += (5 * spirit);
            if (health > maxHealth)
                health = maxHealth; // do not let health be healed above maximum
            mana -= 5;
            manaBar.fillAmount = (mana / maxMana);
            healthBar.fillAmount = (health / maxHealth);
            setSpellButtons();
            endTurn();
        }
        else
            setSpellButtons();
    }
    private void setSpellButtons()
    {
        useHealSpellButton.interactable = false;
        useMageArmorButton.interactable = false;
        useFireballButton.interactable = false;


        if (mana >= 5)
        {
            useHealSpellButton.interactable = true;
        }
        if (mana >= 10)
        {
            useMageArmorButton.interactable = true;
        }
        if (mana >= 15)
        {
            useFireballButton.interactable = true;
        }
        if(mageArmorValue != 0)
        {
            useMageArmorButton.interactable = false;
        }
    }

    public void toggleAttack()
    {
        isAttacking = !isAttacking;
    }
    public void toggleDefend()
    {
        isDeffending = !isDeffending;
    }

    public void wait()
    {
        endTurn();
    }

    public void attacked(byte incomingDamage, byte incomingAccuracey)
    {
        if (Random.Range(1,101) >= (50 - (incomingAccuracey - dodge)))
        {
            if (!isDeffending)
            {
                if ((incomingDamage - (armor + mageArmorValue)) > 0)
                {
                    health -= incomingDamage - (armor + mageArmorValue);
                    healthBar.fillAmount = (health / maxHealth);

                }
            }
            else
            {
                if ((incomingDamage - (armor + mageArmorValue)) > 0)
                {
                    if (stamina > (incomingDamage - (armor + mageArmorValue))) // stamina is used a a health buffer when deffending
                    {
                        stamina -= incomingDamage - (armor + mageArmorValue);
                        if (stamina < 0)
                            stamina = 0;
                        staminaBar.fillAmount = (stamina / maxStamina);
                    }
                    else // if stamina is not enough to absorb the whole blow, remove stamina from damage and then remove remianing damage from health
                    {
                        incomingDamage -= (byte)stamina;
                        stamina = 0;
                        staminaBar.fillAmount = (stamina / maxStamina);
                        health -= incomingDamage - (armor + mageArmorValue);
                        healthBar.fillAmount = (health / maxHealth);
                    }
                }
            }
        } 
        else
        {
            //successful dodge. may use this later for textual discriptions
        }
    }

    public void moveRight()
    {
        if (!isAttacking)
        {
            if (mapRef.checkDestination((int)transform.transform.position.x + 1, (int)transform.transform.position.y))
            {
                transform.Translate(1.0f, 0.0f, 0.0f);
                endTurn();
            }
        }
        else if (isAttacking)
        {
            if (mapRef.checkDestination((int)transform.transform.position.x + 1, (int)transform.transform.position.y))
            {
                transform.Translate(1.0f, 0.0f, 0.0f);
                endTurn();
            }
            else if (mapRef.attackCreature(power, agility, (int)transform.transform.position.x + 1, (int)transform.transform.position.y))
            {
                endTurn();
            }
        }
    }

    public void moveLeft()
    {
        if (!isAttacking)
        {
            if (mapRef.checkDestination((int)transform.transform.position.x - 1, (int)transform.transform.position.y))
            {
                transform.Translate(-1.0f, 0.0f, 0.0f);
                endTurn();
            }
        }
        else if (isAttacking)
        {
            if (mapRef.checkDestination((int)transform.transform.position.x - 1, (int)transform.transform.position.y))
            {
                transform.Translate(-1.0f, 0.0f, 0.0f);
                endTurn();
            }
            else if (mapRef.attackCreature(power, agility, (int)transform.transform.position.x - 1, (int)transform.transform.position.y))
            {
                endTurn();
            }
        }
    }

    public void moveUp()
    {
        if (!isAttacking)
        {
            if (mapRef.checkDestination((int)transform.transform.position.x, (int)transform.transform.position.y + 1))
            {
                transform.Translate(0.0f, 1.0f, 0.0f);
                endTurn();
            }
        }
        else if (isAttacking)
        {
            if (mapRef.checkDestination((int)transform.transform.position.x, (int)transform.transform.position.y + 1))
            {
                transform.Translate(0.0f, 1.0f, 0.0f);
                endTurn();
            }
            else if (mapRef.attackCreature(power, agility, (int)transform.transform.position.x, (int)transform.transform.position.y + 1))
            {
                endTurn();
            }
        }
    }
    public void moveDown()
    {
        if (!isAttacking)
        {
            if (mapRef.checkDestination((int)transform.transform.position.x, (int)transform.transform.position.y - 1))
            {
                transform.Translate(0.0f, -1.0f, 0.0f);
                endTurn();
            }
        }
        else if (isAttacking)
        {
            if(mapRef.checkDestination((int)transform.transform.position.x, (int)transform.transform.position.y - 1))
            {
                transform.Translate(0.0f, -1.0f, 0.0f);
                endTurn();
            }
            else if (mapRef.attackCreature(power, agility, (int)transform.transform.position.x, (int)transform.transform.position.y - 1))
            {
                endTurn();
            }
        }
    }
    public Vector3 getLocation()
    {
        return transform.transform.position;
    }

    // Update is called once per frame
    void Update () {
        if (Input.anyKeyDown)
        {
            Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0.0f);
            if (movement.x > 0) moveRight();
            else if (movement.x < 0) moveLeft();
            else if (movement.y > 0) moveUp();
            else if (movement.y < 0) moveDown();
        }
        if (exitRef)
        {
            if (exitRef.transform.transform.position == transform.transform.position)
            {
                Debug.Log("exit reached");
                mapRef.requestNewLevel();
            }
        }
        else
        {
            exitRef = GameObject.FindGameObjectWithTag("Finish");
        }
    }
}
