using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RubyController : MonoBehaviour
{
    // prev tutorial stuff
    public float speed = 3.0f;
    public int maxHealth = 5;
    public GameObject projectilePrefab;
    public int health { get { return currentHealth; } }
    int currentHealth;
    public float timeInvincible = 2.0f;
    bool isInvincible;
    float invincibleTimer;
    Rigidbody2D rigidbody2d;
    float horizontal;
    float vertical;
    Animator animator;
    Vector2 lookDirection = new Vector2(1, 0);
    AudioSource audioSource;
    public AudioClip cogThrowClip;
    // all new stuff
    public ParticleSystem hearts;
    public ParticleSystem damage;
    bool hasLost;
    bool hasWon;

    // solo adventure cogs
    public int cogs;
    public GameObject totalCogsObject;
    bool hasCogs;
    public TextMeshProUGUI totalCogsText;
    // music
    public AudioClip winClip;
    public AudioClip loseClip;
    public AudioClip hurtClip;
    public AudioClip healClip;

    // text win conditions
    public GameObject loseTextObject;
    public GameObject winTextObeject;
    public GameObject winText2Object;
    public TextMeshProUGUI fixedrobotText;
    private int fixedrobot;

    // restarting 
    public static bool isRoundOne = true;

    // newnew
    public float speedMultiplier = 1;
    bool isInNpcView = false;
    bool isInDoorView = false;
    public GameObject hintText;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        currentHealth = maxHealth;
        audioSource = GetComponent<AudioSource>();
        // goofy new stuff vv
        fixedrobot = 0;
        SetFixedrobotText();
        loseTextObject.SetActive(false);
        winTextObeject.SetActive(false);
        winText2Object.SetActive(false);
        hintText.SetActive(false);

        // cog shizz
        cogs = 0;
        SetTotalCogsText();
    }

    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.R))
        {
            if (hasLost)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                isRoundOne = true;
            }
            else if (hasWon)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        Vector2 move = new Vector2(horizontal, vertical);

        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }

        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);

        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
                isInvincible = false;
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            if (hasCogs)
            {
                Launch();
                audioSource.PlayOneShot(cogThrowClip);
                PickupCog(-1);
            }
        }
        // hints text
        RaycastHit2D seenpc = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("NPC"));
        if (seenpc.collider != null)
        {
            NonPlayerCharacter character = seenpc.collider.GetComponent<NonPlayerCharacter>();
            if (character != null && !isInNpcView)
            {
                isInNpcView = true;
                hintText.SetActive(true);
            }
        }
        RaycastHit2D seedoor = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("door"));
        if (seedoor.collider != null)
        {
            isInDoorView = true;
            hintText.SetActive(true);
            //  Debug.Log("i see door");
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("NPC"));
            if (hit.collider != null)
            {
                NonPlayerCharacter character = hit.collider.GetComponent<NonPlayerCharacter>();
                if (character != null)
                {
                    isInNpcView = true;
                    hintText.SetActive(false);
                    if (isRoundOne)
                    {
                        if (hasWon)
                        {

                            SceneManager.LoadScene("Scene2");
                            isRoundOne = false;
                            winText2Object.SetActive(false);
                            hasWon = false;
                        }
                        else
                        {
                            character.DisplayDialog();
                        }
                    }
                }
            }
        }
        if (isInNpcView)
        {
            RaycastHit2D exithit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("NPC"));
            if (exithit.collider == null)
            {
                hintText.SetActive(false);
                isInNpcView = false;
            }
        }
        if (isInDoorView)
        {
            RaycastHit2D exitdoor = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("door"));
            if (exitdoor.collider == null)
            {
                hintText.SetActive(false);
                isInDoorView = false;
            }
        }
    }

    void FixedUpdate()
    {
        Vector2 position = rigidbody2d.position;
        position.x = position.x + speed * speedMultiplier * (hasLost ? 0 : horizontal) * Time.deltaTime;
        position.y = position.y + speed * speedMultiplier * (hasLost ? 0 : vertical) * Time.deltaTime;
        //   position.x = position.x + speed * horizontal * Time.deltaTime;
        //    position.y = position.y + speed * vertical * Time.deltaTime;

        rigidbody2d.MovePosition(position);
    }
    // new part shit
    void CreateHearts()
    {
        hearts.Play();
        audioSource.PlayOneShot(healClip);
    }
    void CreateDamage()
    {
        damage.Play();
        audioSource.PlayOneShot(hurtClip);
    }

    // cogingtons
    public void PickupCog(int amount)
    {
        cogs += amount;
        SetTotalCogsText();
        hasCogs = cogs > 0;

    }

    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        {
            if (isInvincible)
            {
                return;
            }

            isInvincible = true;
            invincibleTimer = timeInvincible;
            CreateDamage();
        }

        else if (amount > 0)
        {
            CreateHearts();
        }

        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        if (currentHealth == 0)
        {
            hasLost = true;
            loseTextObject.SetActive(true);
            audioSource.PlayOneShot(loseClip);
        }
        UIHealthBar.instance.SetValue(currentHealth / (float)maxHealth);

    }
    public void ChangeSpeed(float amount)
    {
        speedMultiplier = amount;
    }
    // robot 
    public void addFixedRobot()
    {
        fixedrobot += 1;
        SetFixedrobotText();
        if (fixedrobot == 5)
        {
            if (isRoundOne)
            {
                hasWon = true;
                winTextObeject.SetActive(true);
            }
            else
            {
                hasWon = true;
                winText2Object.SetActive(true);
                audioSource.PlayOneShot(winClip);
            }
        }
        //  NonPlayerCharacter npc = other.gameObject.GetComponent<NonPlayerCharacter>();
    }
    void Launch()
    {
        GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);

        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch(lookDirection, 300);

        animator.SetTrigger("Launch");
        audioSource.PlayOneShot(cogThrowClip);

        //   GameObject healthEffect = Instantiate(heathEffect, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);
    }
    void SetFixedrobotText()
    {
        fixedrobotText.text = "Fixed Robots: " + fixedrobot.ToString() + "/5";
    }
    void SetTotalCogsText()
    {
        totalCogsText.text = "Total Cogs: " + cogs.ToString();
    }
}

