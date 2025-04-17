using UnityEngine;

public class CloudMover : MonoBehaviour
{
    public float speed = 0.5f;
    public float resetPositionX = 10f; // d�r molnet startar (t.ex. h�ger om kameran)
    public float leftLimitX = -10f;    // n�r molnet �r ur bild till v�nster

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
