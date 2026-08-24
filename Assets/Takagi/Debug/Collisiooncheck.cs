using UnityEngine;
using UnityEngine.Rendering;

public class Collisiooncheck : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float deltaTime = Time.deltaTime;
        Vector3 pos=transform.position;
        pos.x += deltaTime;
        transform.position=pos; 
    }
}
