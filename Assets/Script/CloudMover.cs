using UnityEngine;

public class CloudMover : MonoBehaviour
{
    public float speed = 0.5f;
    public float resetPositionX = 10f; // där molnet startar (t.ex. höger om kameran)
    public float leftLimitX = -10f;    // när molnet är ur bild till vänster

    void Update()
    {
        transform.Translate(Vector2.left * speed * Time.deltaTime);

        if (transform.position.x < leftLimitX)
        {
            Vector3 newPos = transform.position;
            newPos.x = resetPositionX;
            transform.position = newPos;
        }
    }
}
