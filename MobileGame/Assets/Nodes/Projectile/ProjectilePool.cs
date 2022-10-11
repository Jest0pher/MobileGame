using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ProjectilePool : MonoBehaviour
{
    public GameObject projectilePrefab;

    public int maxSize;

    public List<GameObject> projectiles;
    // Start is called before the first frame update
    void Start()
    {
        projectiles = new List<GameObject>(maxSize);
        for (int i = 0; i < projectiles.Capacity; i++) {
            GameObject obj = Instantiate(projectilePrefab);
            obj.GetComponent<Projectile>().parentPool = this;
            obj.SetActive(false);
            projectiles.Add(obj);
        }
    }

    public GameObject GetItem() {
        for (int i = 0; i < projectiles.Count; i++) {
            if (!projectiles[i].activeSelf) {
                projectiles[i].SetActive(true);
                return projectiles[i];
            }
        }
        print("Pool Maximum Reached");
        return null;
    }

    public void ReturnItem(GameObject obj) {
        if (projectiles.Contains(obj))
        {
            obj.SetActive(false);
        }

    }

}
