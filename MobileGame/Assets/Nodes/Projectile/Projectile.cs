using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Team")]
    private TeamTypes team;
    public TeamTypes Team { 
        get { return team; }
        set { team = Node.SetTeam(value, GetComponent<SpriteRenderer>()); }
    }
    [Space]
    [Header("Movement")]
    public Vector3 velocityDir;
    public Vector3 position;
    public float normalSpeed = .2f;
    [Space]
    public float lifetime;
    public Node node;
    private SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        position = transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        lifetime -= GameManager.Instance.adjustedTimeDelta;
        if (lifetime <= 0)
            Destroy(gameObject);

        position += velocityDir.normalized * normalSpeed * GameManager.Instance.adjustedTimeDelta;

        transform.position = position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<Projectile>(out Projectile otherProj)){
            if (otherProj.Team == Team) {
                return;
            }
        }
        Destroy(gameObject);
    }
}
