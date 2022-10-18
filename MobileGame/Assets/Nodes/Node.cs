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
    [SerializeField] private Node toNode;

    //Variables for runtime
    private int currentAmmo;
    private float currentFireRateTimer;
    private float currentRechargeRateTimer;
    [HideInInspector]
    public int currentHP;
    
    [Space]
    public SpriteRenderer spriteRenderer;
    public CircleCollider2D touchCollider;

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
        toNode = neighborNodes[0];

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

    public Node GetNeighborFromDirection(Vector2 dir) {
        dir.Set((Mathf.Abs(dir.x) < 1 && Mathf.Abs(dir.x) > 0) ? dir.x / Mathf.Abs(dir.x) : dir.x, -1 * ((Mathf.Abs(dir.y) < 1 && Mathf.Abs(dir.y) > 0) ? dir.y / Mathf.Abs(dir.y) : dir.y));
        int index = (int)(gridPos.x + dir.x) + GameManager.Instance.grid.columnCount * (int)(gridPos.y + dir.y);
        Node potentialNeighbor = GameManager.Instance.grid.nodes[index];
        if (neighborNodes.Contains(potentialNeighbor)){
            return potentialNeighbor;
        }
        return null;
    }

    public void SetToNode(Node _toNode) {
        if (neighborNodes.Contains(_toNode))
            toNode = _toNode;
    }
}

