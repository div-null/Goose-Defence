using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MiniCollider : MonoBehaviour
{
    public List<Sprite> WallSprites;
    public GameObject hpBar;
    // Start is called before the first frame update
    void Start()
    {
		parent = transform.parent.GetComponent<Tower>();        
    }
	Tower parent;


	private void OnTriggerEnter(Collider other)
	{
		parent.OnTriggerEnter(other);
	}
	// Update is called once per frame
	void Update()
    {
        if (gameObject.CompareTag("Wall"))
        {
            hpBar.transform.localScale = new Vector3(parent.HP / parent.maxHP * 4, 0.57f, 1);
            //hpBar.transform.position -= new Vector3(parent.HP / parent.maxHP * 4, 0, 0);
            if (parent.HP == parent.maxHP)
            {
                parent.gameObject.GetComponentInChildren<SpriteRenderer>().sprite = WallSprites[0];
            }
            else if (parent.HP < parent.maxHP / 2)
            {
                parent.gameObject.GetComponentInChildren<SpriteRenderer>().sprite = WallSprites[1];
            }
            if (parent.HP <= 0)
            {
                parent.gameObject.GetComponentInChildren<SpriteRenderer>().sprite = WallSprites[2];
            }
        }
    }
}
