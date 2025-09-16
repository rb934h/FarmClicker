using UnityEngine;

public class AnimalWalker : MonoBehaviour
{
    [SerializeField] private float speed = 2f;  
    [SerializeField] private Vector2 minBounds;     
    [SerializeField] private Vector2 maxBounds;       

    private Vector2 targetPosition;
    private SpriteRenderer spriteRenderer;
   // private Animator animator;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
      //  animator = GetComponent<Animator>();
        ChooseNewTarget();
    }

    void Update()
    {
        Vector2 oldPos = transform.position;
        
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        Vector2 velocity = ((Vector2)transform.position - oldPos) / Time.deltaTime;
        
        if (velocity.x > 0.01f)
            spriteRenderer.flipX = true;
        else if (velocity.x < -0.01f)
            spriteRenderer.flipX = false;

        // передаём скорость в Animator
     //   animator.speed = velocity.magnitude;
     
        if (Vector2.Distance(transform.position, targetPosition) < 0.1f)
            ChooseNewTarget();
    }

    void ChooseNewTarget()
    {
        targetPosition = new Vector2(
            Random.Range(minBounds.x, maxBounds.x),
            Random.Range(minBounds.y, maxBounds.y)
        );
    }
}