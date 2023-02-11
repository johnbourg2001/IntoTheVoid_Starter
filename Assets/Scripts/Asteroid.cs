using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public Sprite[] sprites;
    public float size = 1.0f;
    public float minSize = 0.5f;
    public float maxSize = 1.5f;
    public float speed = 50.0f;
    public float maxLifeTime = 30.0f;

    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rigidBody;

    private float largeCutoff;
    private float mediumCutoff;
    private float smallCutoff;
    private float extraSpawnChance = 0.2f; // 20% chance to spawn an extra, smallest asteroid on largest asteroids

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rigidBody = GetComponent<Rigidbody2D>();  

    }

    // Start is called before the first frame update
    private void Start()
    {
        _spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)];
        
        this.transform.eulerAngles = new Vector3(0.0f, 0.0f, Random.value * 360.0f);
        this.transform.localScale = Vector3.one * this.size;
        
        _rigidBody.mass = this.size;

        float sizeRange = this.maxSize - this.minSize;
        float sizeStep = sizeRange/3;
        this.largeCutoff = this.maxSize - sizeStep;
        this.mediumCutoff = largeCutoff - sizeStep;
        this.smallCutoff = mediumCutoff - sizeStep;
    }

    public void SetTrajectory(Vector2 direction)
    {
        _rigidBody.AddForce(direction * this.speed);

        Destroy(this.gameObject, this.maxLifeTime);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            if (this.size >= this.mediumCutoff)
            {
                CreateSplit(this.size * 0.5f);
                CreateSplit(this.size * 0.5f);        
            }
            if (this.size >= (this.maxSize * 0.9f)) // On largest asteroids (90%+ max size)
            {
                if (Random.Range(0.0f, 1.0f) < extraSpawnChance) // 20% chance to spawn an extra, smallest asteroid
                {
                    CreateSplit(this.minSize);
                }
            }
            Destroy(this.gameObject);
        }
    }

    private void CreateSplit(float size)
    {
        Vector2 position = this.transform.position;
        position += Random.insideUnitCircle * 0.5f;

        Asteroid half = Instantiate(this, position, this.transform.rotation);
        half.size = size;
        half.SetTrajectory(Random.insideUnitCircle.normalized);
    }

}
