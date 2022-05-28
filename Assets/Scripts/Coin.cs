using UnityEngine;

public class Coin : MonoBehaviour
{
    public PowerupEffect powerupEffect;
    private GameObject player;
    private Vector3 randomPos;
    private Vector3 randomDir;
    private bool playerIsTarget;
    private float speed;
    private float initialSpeed = 2.5f;
    private float nextSpeed = 100;
    private float t = 0;

    private void Start()
    {
        player = PlayerStats.instance.gameObject;
        speed = initialSpeed;
    }

    private void OnEnable()
    {
        playerIsTarget = false;
        speed = initialSpeed;
        t = 0;
        GetRandomPos();
    }

    private void Update()
    {
        if (playerIsTarget)
        {
            TranslateToPlayer();
            return;
        }
        TranslateToInitialPos();
    }

    private void TranslateToInitialPos()
    {
        transform.position = Vector3.Lerp(transform.position, randomPos, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, randomPos) < 0.1f) playerIsTarget = true;
    }

    private void TranslateToPlayer()
    {
        speed = Mathf.Lerp(speed, nextSpeed, t);
        t += 0.5f * Time.deltaTime;

        Vector3 direction = player.transform.position - transform.position;
        direction = Vector3.ClampMagnitude(direction, 1);
        transform.Translate(direction * speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, player.transform.position) < 0.1f)
        {
            powerupEffect.Apply(player);
            gameObject.SetActive(false);
        }
    }

    private void GetRandomPos()
    {
        randomDir = new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));
        randomDir = ClampMagnitude(randomDir, 2, 8);
        randomPos = transform.position + randomDir;
    }

    private Vector3 ClampMagnitude(Vector3 _vector, float _min, float _max)
    {
        double sqrMagnitude = _vector.sqrMagnitude;
        if (sqrMagnitude > (double)_max * (double)_max) return _vector.normalized * _max;
        else if (sqrMagnitude < (double)_min * (double)_min) return _vector.normalized * _min;
        return _vector;
    }
}
