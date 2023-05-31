using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    Transform target;
    Material material;
    Vector2 offset = Vector2.zero;
    [SerializeField] float scale = 1.0f;
    void Start()
    {
        target = transform.root;
        material = GetComponent<SpriteRenderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        offset = new Vector2(target.position.x / scale, 0f);
        material.mainTextureOffset = offset;
    }
}
