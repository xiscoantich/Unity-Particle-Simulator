using UnityEngine;

public class ParticleScript : MonoBehaviour
{
    public Rigidbody2D myRigidbody;
    public float deathRegion = 10.0f;

    private Vector2 deathPoint;
    private Vector2 initialPosition;
    private SpriteRenderer spriteRenderer;

    public void SetParticleData(Vector2 spawnPosition, Vector2 deathPosition)
    {
        initialPosition = spawnPosition;
        deathPoint = deathPosition;

        // Move the particle to the initial position
        transform.position = new Vector3(initialPosition.x, initialPosition.y, transform.position.z);
    }

    private void Start()
    {
        transform.position = new Vector3(initialPosition.x, initialPosition.y, transform.position.z);
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        ParticleDestructor();
        AtractionForce();
        ComputeColor();
    }

    private void AtractionForce()
    {
        if (myRigidbody != null)
        {
            Vector2 particlePosition = transform.position;
            //float deathDistance = SquareDistance(particlePosition, deathPoint);
            Vector2 distanceVec = DistanceVec(particlePosition, deathPoint);
            distanceVec.Normalize();

            myRigidbody.AddForce(100f * distanceVec * Time.deltaTime);
        }
    }

    private void ParticleDestructor()
    {
        Vector2 particlePosition = transform.position;
        float deathDistance = SquareDistance(particlePosition, deathPoint);
        if (deathDistance < deathRegion * deathRegion)
        {
            Destroy(gameObject);
        }
    }

    private void ComputeColor()
    {
        Vector2 velocity = myRigidbody.velocity;
        float speed = velocity.magnitude;
        float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
        if (angle < 0)
            angle += 360f;
        float hue = angle / 360f;
        float saturation = Mathf.Lerp(1f, 0.1f, speed / 5f);
        float brightness = 1f;

        Color particleColor = Color.HSVToRGB(hue, saturation, brightness);
        spriteRenderer.color = particleColor;
    }

    private float SquareDistance(Vector2 point1, Vector2 point2)
    {
        float xDifference = point1.x - point2.x;
        float yDifference = point1.y - point2.y;
        return xDifference * xDifference + yDifference * yDifference;
    }

    private Vector2 DistanceVec(Vector2 point1, Vector2 point2)
    {
        return point2 - point1;
    }
}

