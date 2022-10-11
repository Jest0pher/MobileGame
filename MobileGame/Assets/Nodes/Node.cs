using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TeamTypes {
    Neutral,
    Red,
    Green,
    Blue,
    Purple,
    Orange,
    Yellow,
    Count
}

/*public enum ProjectileType { 
    X1 = 0,
    X2 = 2,
    X3 = 4,
    X4,
    X5
}*/
public class Node : MonoBehaviour
{
    [Header("Projectile")]
    [SerializeField] GameObject projectilePrefab;
    [Space]
    [Header("Team")]
    private TeamTypes team;
    public TeamTypes Team { 
        get { return team; }
        set { team = SetTeam(value, spriteRenderer); }
    }
    [Space]
    [Header("Properties")]
    public int maxAmmo;
    public float fireRate;
    public float rechargeRate;
    [Space]
    public int maxHP;

    [Space]
    public List<Node> neighborNodes;
    private Node toNode;

    //Variables for runtime
    private int currentAmmo;
    private float currentFireRateTimer;
    private float currentRechargeRateTimer;
    [HideInInspector]
    public int currentHP;
    
    [Space]
    public SpriteRenderer spriteRenderer;

    [Space]
    public Vector2 gridPos;
    //Set Team
    [Space]
    [Range(0, 6)]
    public int teamNum;
    private int prevNum;
    // Start is called before the first frame update
    void Start()
    {
        currentAmmo = maxAmmo;
        currentFireRateTimer = fireRate;
        currentRechargeRateTimer = rechargeRate;
        currentHP = Team == TeamTypes.Neutral ? Random.Range(1, maxHP+1) : maxHP;

        //int randomTeam = Random.Range(0, 6);
        //team = SetTeam((TeamTypes)randomTeam, spriteRenderer);
        
        teamNum = (int)team;
        prevNum = teamNum;
    }

    // Update is called once per frame
    void Update()
    {
        if (prevNum != teamNum) {
            prevNum = teamNum;
            Team = (TeamTypes)teamNum;
        }

        currentRechargeRateTimer -= GameManager.Instance.adjustedTimeDelta;
        if (currentRechargeRateTimer <= 0) {
            currentAmmo = Mathf.Clamp(currentAmmo+1, 0, maxAmmo);
            currentRechargeRateTimer = rechargeRate;
        }

        if (currentFireRateTimer <= 0)
        {
            if(currentAmmo > 0) { 
                Shoot();
                currentFireRateTimer = fireRate;
            }
        }
        else { 
            currentFireRateTimer -= GameManager.Instance.adjustedTimeDelta;
        }
    }

    void Shoot() {
        if (Team == TeamTypes.Neutral)
            return;

        //GameObject projectileObj = Instantiate(projectilePrefab, transform.position, transform.rotation);
        GameObject projectileObj = GameManager.Instance.projectiles.GetItem();
        if(projectileObj != null)    
        {
            projectileObj.transform.position = transform.position;
            Projectile projectile = projectileObj.GetComponent<Projectile>();
            projectile.position = transform.position;
            projectile.node = this;
            projectile.Team = team;
            int neighborIndex = Random.Range(0, neighborNodes.Count);
            toNode = neighborNodes[neighborIndex];
            projectile.velocityDir = toNode.transform.position - transform.position;
            currentAmmo--;
        }
    }

    public static TeamTypes SetTeam(TeamTypes _team, SpriteRenderer _spriteRenderer) {
        switch (_team)
        {
            case TeamTypes.Neutral:
                _spriteRenderer.color = Color.white;
                break;
            case TeamTypes.Red:
                _spriteRenderer.color = Color.red;
                break;
            case TeamTypes.Green:
                _spriteRenderer.color = Color.green;
                break;
            case TeamTypes.Blue:
                _spriteRenderer.color = Color.blue;
                break;
            case TeamTypes.Purple:
                _spriteRenderer.color = Color.magenta;
                break;
            case TeamTypes.Orange:
                _spriteRenderer.color = new Color(1, .5f, 0, 1);
                break;
            case TeamTypes.Yellow:
                _spriteRenderer.color = Color.yellow;
                break;
            default:
                break;
        }
        return _team;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Projectile")
        {
            Projectile projectile = collision.GetComponent<Projectile>();
            if (projectile.node == this)
                return;

            if (projectile.Team == Team)
            {
                if (currentHP == maxHP)
                {
                    currentAmmo = Mathf.Clamp(currentAmmo + 1, 0, maxAmmo);
                    Shoot();
                }
                else { 
                    currentHP = Mathf.Clamp(currentHP + 1, 0, maxHP);
                }
            }
            else {
                currentHP = Mathf.Clamp(currentHP - 1, 0, maxHP);
                if (currentHP == 0) {
                    currentHP = 1;
                    GameManager.Instance.teamCounts[Team]--;
                    GameManager.Instance.teamCounts[projectile.Team]++;
                    Team = projectile.Team;
                }
            }
            GameManager.Instance.projectiles.ReturnItem(projectile.gameObject);
            //Destroy(projectile.gameObject);
        }
    }
}

