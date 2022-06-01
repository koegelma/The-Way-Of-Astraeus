using UnityEngine;
using UnityEngine.UI;

public class DamageUI : MonoBehaviour
{
    [SerializeField] private Text damageAmount;
    private float scaleFactor;
    private float speed = 5;

    private void OnEnable()
    {
        scaleFactor = 1;
        transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
    }

    private void OnDisable()
    {
        damageAmount.text = null;
    }

    private void Update()
    {
        scaleFactor -= Time.deltaTime;
        if (scaleFactor < 0)
        {
            scaleFactor = 0;
            gameObject.SetActive(false);
        }
        transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    public void SetDamageAmount(float _damageAmount)
    {
        damageAmount.text = Mathf.Round(_damageAmount).ToString();
    }

    public void SetColor(Color _color)
    {
        damageAmount.color = _color;
    }
}
