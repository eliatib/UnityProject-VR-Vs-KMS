using Project.Entities;
using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(Rigidbody))]
public class BulletProjectile : MonoBehaviourPunCallbacks
{
    private float forceAmmount = 20f;
    public int damage = 2;
    private Rigidbody rb;
    public EntityBase caster;
    private Renderer renderer;

    private void Awake()
    {
        renderer = GetComponent<MeshRenderer>();
        rb = GetComponent<Rigidbody>();
        //AudioManager.Instance.Play("Tir");
        FindObjectOfType<AudioManager>().Play("Tir");
    }

    void Start()
    {
        InitializeColor();

        rb.velocity = (transform.up*forceAmmount);
        Destroy(gameObject, 3);
    }


    void InitializeColor()
    {
        GameConfig.Instance.LoadJson();
        Color outColor = Color.white;
        if (caster is Virus)
        {

            ColorUtility.TryParseHtmlString(GameConfig.Instance.colorShotVirus, out outColor);
        }
        else if (caster is Scientist)
        {
            ColorUtility.TryParseHtmlString(GameConfig.Instance.colorShotKMS, out outColor);

        }
        else
        {
            Debug.LogError("Warning, entity not register in bulletProjectile, impossible to give a color");
        }
        renderer.material.color = outColor;

    }
    private void OnCollisionEnter(Collision collision)
    {
        EntityBase target = collision.transform.GetComponent<EntityBase>();
        if (collision.transform.tag.Equals("BodyVirus"))
        {
            target = collision.transform.parent.parent.GetComponent<EntityBase>();
        }

        if(target != null && ((target is Scientist && caster is Virus) || (caster is Scientist && target is Virus)))
        {
            FindObjectOfType<AudioManager>().Play("Touche");
            //AudioManager.Instance.Play("Touche");
            target.photonView.RPC("TakeDamage", RpcTarget.All,damage);

           
            Debug.Log("Deal 2 dmg to " + target.gameObject.name);
        }
        Destroy(gameObject);
    }
}
