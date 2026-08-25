using UnityEngine;
using UnityEngine.Rendering;

public class Collisiooncheck : MonoBehaviour
{
    [SerializeField]private Rigidbody2D rb=null;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        Vector2 vec = new Vector2(250, 0);
        if(rb)rb.AddForce(vec);
    }

    // Update is called once per frame
    void Update()
    {
        float deltaTime = Time.deltaTime;
    }
}
